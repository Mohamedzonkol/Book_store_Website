namespace Book.Models.ViewModel
{
	public class OrderVM
	{

		public OrderHeader OrderHeader { get; set; }
		public IEnumerable<OrderDetail> OrderDetail { get; set; }
	}
}
