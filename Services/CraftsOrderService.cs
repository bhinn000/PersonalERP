using PersonalERP.DTO;
using PersonalERP.Entity;
using PersonalERP.Interface;
using PersonalERP.Interfaces;

namespace PersonalERP.Services
{
    public class CraftsOrderService : ICraftsOrderService
    {
        private readonly ICraftsOrderRepo _craftsOrderRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IArtPieceRepo _artPieceRepo;
        //private readonly IMenuItemRepo _menuItemRepo;
        //private readonly IMenuOrderDetailRepo _menuOrderDetailRepo;
        //private readonly IOrganizationRepository _organizationRepo;
        //private readonly IAuthRepository _authRepository;
        //private readonly IMenuIdentityRepo _menuIdentityRepo;
        //private readonly IDiscountRepo _discountRepo;
        //private readonly IPrintLogRepo _printLogRepo;
        public CraftsOrderService(
            ICraftsOrderRepo craftsOrderRepo,
             ICustomerRepo customerRepo,
             IArtPieceRepo artPieceRepo
            //IMenuItemRepo menuItemRepo,
            //IMenuOrderDetailRepo menuOrderDetailRepo,
            //IOrganizationRepository organizationRepository,
            //IAuthRepository authRepository,
            //IMenuIdentityRepo menuIdentityRepo,
            //IDiscountRepo discountRepo,
            //IPrintLogRepo printLogRepo
            )
        {
            _craftsOrderRepo = craftsOrderRepo;
            _customerRepo = customerRepo;
            //_menuItemRepo = menuItemRepo;
            //_menuOrderDetailRepo = menuOrderDetailRepo;
            //_organizationRepo = organizationRepository;
            //_authRepository = authRepository;
            //_menuIdentityRepo = menuIdentityRepo;
            //_discountRepo = discountRepo;
            //_printLogRepo = printLogRepo;
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
                if (orderedArtInfo != null)
                {
                    throw new Exception("There doesnt exist that Art Piece");
                }

                Customer? theCustomer;
                if (string.IsNullOrWhiteSpace(createCraftsOrderDto.PhoneNum))
                    throw new Exception("Phone number is required.");

                var oldCustomer = await _customerRepo.GetByPhN(createCraftsOrderDto.PhoneNum);

                //to check if customer is old or new

                if (oldCustomer is not null) //if old customer
                {
                    theCustomer = oldCustomer;
                }
                else // if new customer
                    {
                        if (string.IsNullOrWhiteSpace(createCraftsOrderDto.CustomerName) ||
                        string.IsNullOrWhiteSpace(createCraftsOrderDto.Address) || 
                        string.IsNullOrWhiteSpace(createCraftsOrderDto.CustomerId))
                        {
                            throw new Exception("Seem like new customer , please give all information");
                        }
                        theCustomer = new Customer
                        {
                            UserId = createCraftsOrderDto.CustomerId,
                            Name= createCraftsOrderDto.CustomerName,
                            Address = createCraftsOrderDto.Address,
                            PhoneNum= createCraftsOrderDto.PhoneNum,
                            TotalBillAmount=orderedArtInfo.Price,
                            TotalBillPayable=orderedArtInfo.Price,
                        };

                    }

                var newOrder = new CraftsOrder
                {
                    CustomerId = theCustomer.Id,
                    OrderRef = createCraftsOrderDto.OrderRef,
                    ArtId = createCraftsOrderDto.ArtId,
                    ArtName = orderedArtInfo.Name,
                    Price = orderedArtInfo.Price,
                    Description = orderedArtInfo.Description,
                };

                return await _craftsOrderRepo.CreateAsync(newOrder);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the order: {ex.Message}", ex);
            }
        }

        public async Task<CraftsOrder?> GetByIdAsync(int id)
        {

            var craftsOrder = await _craftsOrderRepo.GetByIdAsync(id);
            if (craftsOrder == null)
            {
                throw new KeyNotFoundException("There is no specific crafts order info");
            }
            return craftsOrder;


        }

        public async Task<IEnumerable<CraftsOrder>> GetAllAsync()
        {
            try
            {
                return await _craftsOrderRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting the order: {ex.Message}", ex);
            }
        }

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