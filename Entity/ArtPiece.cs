namespace PersonalERP.Entity
{
    public class ArtPiece : DateTimeEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? CraftsOrderId { get; set; }//shadow foreign key
        public virtual CraftsOrder? CraftsOrder { get; set; }

        
    }
}
