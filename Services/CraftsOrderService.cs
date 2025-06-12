using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;

namespace PersonalERP.Services
{
    public class CraftsOrderService : ICraftsOrderService
    {
        private readonly ICraftsOrderRepo _craftsOrderRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IArtPieceRepo _artPieceRepo;
        private readonly IUserContextService _userContextService;
        private readonly IBillPaymentCreditRepo _billPaymentCreditRepo;

        public CraftsOrderService(
            ICraftsOrderRepo craftsOrderRepo,
             ICustomerRepo customerRepo,
             IArtPieceRepo artPieceRepo,
             IUserContextService userContextService,
             IBillPaymentCreditRepo billPaymentCreditRepo
            )
        {
            _craftsOrderRepo = craftsOrderRepo;
            _customerRepo = customerRepo;
            _artPieceRepo = artPieceRepo;
            _userContextService = userContextService;
            _billPaymentCreditRepo = billPaymentCreditRepo;
        }



        //public async Task<int> CreateAsync(CreateCraftsOrderDto inputDto, int cred)
        //{
        //    try
        //    {
        //        //await ValidateInputDataAsync(inputDto.Items);


        //        //var user = await _authRepository.GetUserByUsername(cred);
        //        //if (user == null)
        //        //{
        //        //    user = await _authRepository.GetUserByEmailAddress(cred);
        //        //}

        //        //check if art and customer is available (if customer doesnt exist, crete the record)

        //        var menuItemIds = inputDto.Items.Select(x => x.MenuItemId).ToList();
        //        var menuItems = await _menuItemRepo.GetDetailsAsync(menuItemIds);

        //        var menuOrderDetails = inputDto.Items
        //            .Select(x =>
        //            {
        //                var itemDetail = menuItems.FirstOrDefault(m => m.Id == x.MenuItemId);
        //                if (itemDetail == null)
        //                {
        //                    throw new ArgumentException($"Menu item with ID {x.MenuItemId} not found.");
        //                }

        //                return new MenuOrderDetail
        //                {
        //                    MenuItemId = x.MenuItemId,
        //                    Quantity = x.Quantity,
        //                    Price = (double)itemDetail.Price,
        //                };
        //            })
        //            .ToList();

        //        string refNo = GenerateUniqueOrgCode();

        //        var TaxData = await GetOrgCharges(orgCode);

        //        double ServiceCharge = (TaxData != null && TaxData.ServiceCharge > 0)
        //            ? (double)(menuOrderDetails.Sum(x => x.Price) * (double)TaxData.VAT / 100)
        //            : 0;

        //        var menuOrder = new MenuOrder
        //        {
        //            OrderRef = refNo,
        //            DiscountAmount = 0,
        //            OrderStatus = OrderStatusEnum.Ongoing,
        //            OrderType = OrderTypeEnum.QRMenu,
        //            TotalAmount = menuOrderDetails.Sum(x => x.Price),
        //            FinalAmount = menuOrderDetails.Sum(x => x.Price),
        //            OrganizationId = organization.Id,
        //            UserId = user != null ? user.Id : "",
        //            MenuOrderDetails = menuOrderDetails,
        //            ServiceCharge = ServiceCharge,
        //            ServiceChargePercentage = (TaxData != null) ? (double)TaxData.ServiceCharge : 0,
        //            TaxPercentage = (TaxData != null) ? (double)TaxData.VAT : 0,
        //            TaxAmount = (TaxData != null && TaxData.VAT > 0)
        //                ? ((double)(ServiceCharge + menuOrderDetails.Sum(x => x.Price)) * (double)TaxData.VAT / 100)
        //                : 0
        //        };

        //        menuOrder.MenuOrderStatusLogs.Add(new MenuOrderStatusLog
        //        {
        //            CreatedBy = "admin",
        //            CreatedDate = DateTime.UtcNow,
        //            OrderStatus = OrderStatusEnum.Ongoing,
        //            PreviousStatus = OrderStatusEnum.Ongoing,
        //            Remarks = "Order created.",
        //        });

        //        _orderRepo.Insert(menuOrder);
        //        await _orderRepo.SaveAsync();

        //        return menuOrder.Id;
        //        //return mapMenuOrderDetail(menuOrder);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred while creating the order: {ex.Message}", ex);
        //    }
        //}

        public async Task<int> CreateAsync(CreateCraftsOrderDto createCraftsOrderDto)
        {
            try
            {
                var orderedArtInfo = await _artPieceRepo.GetArtPiece(createCraftsOrderDto.ArtId);//artInfo
                if (orderedArtInfo != null && orderedArtInfo.CraftsOrderId is not null)
                {
                    throw new Exception("There doesnt exist that Art Piece or it has already been reserved");
                }

                Customer? theCustomer;
                if (string.IsNullOrWhiteSpace(createCraftsOrderDto.PhoneNum))
                    throw new Exception("Phone number is required.");

                var oldCustomer = await _customerRepo.GetByPhN(createCraftsOrderDto.PhoneNum);

                //to check if customer is old or new

                if (oldCustomer is not null) //if old customer
                {
                    theCustomer = oldCustomer;
                    var totalValueForOrderedArtYet = orderedArtInfo.Price + theCustomer.TotalBillAmount;
                    var totalBillPayableRemForOne = orderedArtInfo.Price + theCustomer.TotalBillPayable - createCraftsOrderDto.InitialPayment??0m;

                    //Id = theCustomer.Id,
                    theCustomer.TotalBillAmount = totalValueForOrderedArtYet;
                    theCustomer.TotalBillPayable = totalBillPayableRemForOne;
                    theCustomer.TotalBillPaid = (theCustomer.TotalBillPaid??0m) + createCraftsOrderDto.InitialPayment;

                    //theCustomer.CurrentCreditLimit = (theCustomer.CurrentCreditLimit ?? 0m)
                    //    - orderedArtInfo.Price + (createCraftsOrderDto.InitialPayment ?? 0m);

                    //await _customerRepo.UpdateAsync(theCustomer);

                    // Use InitialCreditLimit if CurrentCreditLimit is null
                    var baseCredit = theCustomer.CurrentCreditLimit ?? theCustomer.InitialCreditLimit;
                    var updatedCredit = baseCredit - orderedArtInfo.Price + (createCraftsOrderDto.InitialPayment ?? 0m);

                    if (updatedCredit < 0)
                    {
                        throw new Exception("Insufficient credit limit.");
                    }

                    theCustomer.CurrentCreditLimit = updatedCredit;

                    await _customerRepo.UpdateAsync(theCustomer);
                }
                else // if new customer
                {
                    if (string.IsNullOrWhiteSpace(createCraftsOrderDto.CustomerName) ||
                    string.IsNullOrWhiteSpace(createCraftsOrderDto.Address) ||
                     (createCraftsOrderDto.InitialCreditLimit ?? 0) <= 0.0m)
                    {
                        throw new Exception("Seem like new customer , please give all information");
                    }

                    theCustomer = new Customer
                    {
                        UserId = createCraftsOrderDto.CustomerId,
                        Name = createCraftsOrderDto.CustomerName,
                        Address = createCraftsOrderDto.Address,
                        PhoneNum = createCraftsOrderDto.PhoneNum,
                        TotalBillAmount = orderedArtInfo.Price,
                        TotalBillPaid=createCraftsOrderDto.InitialPayment,
                        TotalBillPayable = orderedArtInfo.Price - (  createCraftsOrderDto.InitialPayment ?? 0m),
                        InitialCreditLimit = createCraftsOrderDto.InitialCreditLimit.Value,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser",
                        CurrentCreditLimit = createCraftsOrderDto.InitialCreditLimit.Value
                        - orderedArtInfo.Price + (createCraftsOrderDto.InitialPayment ?? 0m),

                    };
                    await _customerRepo.AddAsync(theCustomer);
                }
              

                var newOrder = new CraftsOrder
                {
                    CustomerId = theCustomer.Id,
                    OrderRef = createCraftsOrderDto.OrderRef,
                    ArtId = createCraftsOrderDto.ArtId,
                    ArtName = orderedArtInfo.Name,
                    Price = orderedArtInfo.Price,
                    Description = orderedArtInfo.Description,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser",

                };

                var craftsOrder=await _craftsOrderRepo.CreateAsync(newOrder);

                var newBPaidRecord = new BillPaymentCredit
                { 
                    BillAmount = orderedArtInfo.Price,
                    PaidAmount=createCraftsOrderDto.InitialPayment,
                    PaymentReceivable=orderedArtInfo.Price - createCraftsOrderDto.InitialPayment,
                    CustomerId=theCustomer.Id,
                    IsInitialPayment=true,
                    CraftsOrderId=newOrder.Id,
                    CompletelyPaid= (orderedArtInfo.Price - (createCraftsOrderDto.InitialPayment ?? 0m)) == 0m,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser",

                };

                var newBPaidRec= await _billPaymentCreditRepo.AddAsync(newBPaidRecord);

                if(newBPaidRec is not null)
                {
                    newOrder.BillPaymentId = newBPaidRecord.Id;
                    await _craftsOrderRepo.UpdateAsync(newOrder);
                }
             

                var artPiece = await _artPieceRepo.GetArtPiece(createCraftsOrderDto.ArtId);

                if (artPiece != null)
                {
                    artPiece.CraftsOrderId = newOrder.Id;
                    artPiece.ModifiedDate = DateTime.UtcNow;
                    artPiece.ModifiedBy = _userContextService.GetCurrentUsername() ?? "UnknownUser";

                    await _artPieceRepo.UpdateAsync(artPiece);
                }


                return craftsOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the order: {ex.Message}", ex);
            }
        }

        public async Task<CraftsOrderDTO?> GetByIdAsync(int id)
        {

            var craftsOrder = await _craftsOrderRepo.GetByIdAsync(id);
            if (craftsOrder == null)
            {
                throw new KeyNotFoundException("There is no specific crafts order info");
            }
            return new CraftsOrderDTO
            {
                Id = craftsOrder.Id,
                OrderRef = craftsOrder.OrderRef,
                ArtName = craftsOrder.ArtName,
                Price = craftsOrder.Price,
                Description = craftsOrder.Description,
                CustomerName = craftsOrder.Customer?.Name,
                CustomerId = craftsOrder.CustomerId,
                ArtId = craftsOrder.ArtId
            };
        }

        public async Task<IEnumerable<CraftsOrderDTO>> GetAllAsync()
        {
            try
            {
                var orders= await _craftsOrderRepo.GetAllAsync();
                return orders.Select(order => new CraftsOrderDTO
                 {
                     Id = order.Id,
                     OrderRef = order.OrderRef,
                     ArtName = order.ArtName,
                     Price = order.Price,
                     Description = order.Description,
                     CustomerName = order.Customer.Name,
                     CustomerId=order.CustomerId,
                     ArtId=order.ArtId
                 });
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting the order: {ex.Message}", ex);
            }
        }

        //public async Task<IEnumerable<CraftsOrderDTO>> GetAllAsync()
        //{
        //    var orders = await _appDbContext.CraftsOrders
        //                    .Include(c => c.Customer)
        //                    .Include(a => a.Art)
        //                    .ToListAsync();

        //    return orders.Select(order => new CraftsOrderDTO
        //    {
        //        Id = order.Id,
        //        OrderRef = order.OrderRef,
        //        ArtName = order.ArtName,
        //        Price = order.Price,
        //        Description = order.Description,
        //        CustomerName = order.Customer.Name
        //    });
        //}


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _craftsOrderRepo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the order: {ex.Message}", ex);
            }
        }
    }
}