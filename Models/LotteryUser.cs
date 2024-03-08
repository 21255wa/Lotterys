namespace Lottery.Models
{
	public class LotteryUser
	{
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UserId { get; set; }
		/// <summary>
		/// 用户昵称
		/// </summary>

		public string UserName { get; set; }
		/// <summary>
		/// 微信号
		/// </summary>
		public string WxNum { get; set; }
		/// <summary>
		/// 公司名称
		/// </summary>

		public string CompanyName { get; set; }
		/// <summary>
		///抽奖号码
		/// </summary>
		public int LotteryNum { get; set; }
		/// <summary>
		/// 抽奖Id
		/// </summary>
		public int LotteryId { get; set; }
	}
}
