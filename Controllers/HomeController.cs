using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Lottery.Models;
using Lottery.DBFirst;
using Microsoft.EntityFrameworkCore;

namespace Lottery.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly LotteryDbContext _context;

		public HomeController(ILogger<HomeController> logger, LotteryDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		public IActionResult LotterysIndex()
		{
			return View();
		}
		public IActionResult UesrIndex() 
		{
			return View();
		}
		/// <summary>
		/// 添加用户信息
		/// </summary>
		/// <param name="lotteryUser"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> getInputValue(LotteryUser lotteryUser)
		{
			
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					// 检查数据库中是否已经存在相同的数据
					bool isDuplicate = await _context.lotteryUsers.AnyAsync(u => u.WxNum == lotteryUser.WxNum);

					if (isDuplicate)
					{
						// 如果存在重复数据，可以返回相应的错误信息或者进行其他处理
						ModelState.AddModelError("DuplicateData", "该用户已经存在");
						return View(); // 返回到原页面或者其他处理方式
					}
					else
					{
						//获取号码
						var LotteryNum =await GetLotteryUserCount();
						if (LotteryNum != null) {
							lotteryUser.LotteryNum = int.Parse(LotteryNum);

						}
						// 如果不存在重复数据，插入新数据
						_context.lotteryUsers.Add(lotteryUser);


						bool isDuplicateNum = await _context.lotteryTables.AnyAsync(u => u.LotteryNum == lotteryUser.LotteryNum);//判断中将表是否有重复数据
						if (!isDuplicateNum)
						{

							LotteryTable lotteryTable = new LotteryTable();
							lotteryTable.LotteryNum = lotteryUser.LotteryNum;
							lotteryTable.LotteryMessage = GenerateWinningMessage();
							_context.lotteryTables.Add(lotteryTable);
						}
						await _context.SaveChangesAsync();
						return RedirectToAction("Index");
					}
				}
				catch (Exception ex)
				{
					transaction.Rollback(); // 回滚事务
											// 可以添加日志记录或其他处理逻辑
					ModelState.AddModelError(string.Empty, "发生错误，操作已回滚");
					return View();
				}
			}
		}
		// 生成中奖信息的方法
		private string GenerateWinningMessage()
		{
			// 这里可以根据具体需求生成中奖信息，比如随机生成、根据规则生成等
			// 这里只是一个示例，实际中需要根据业务需求来编写相应的逻辑
			Random random = new Random();
			int randomWinningNumber = random.Next(1,5); // 生成 1 到 100 之间的随机数作为中奖信息
			string message = randomWinningNumber + "等奖";
			return message;
		}
		/// <summary>
		/// 生成抽奖号码(根据用户的条数进行)
		/// </summary>
		/// <returns></returns>W
		public async Task<string> GetLotteryUserCount()
		{
			int count = await _context.lotteryUsers.CountAsync(); // 获取数据条数
			string formattedCount = count+1.ToString("D4"); // 将数字格式化为四位数的字符串
			return formattedCount;
		}

		/// <summary>
		/// 根据微信号获取抽奖号码
		/// </summary>
		/// <param name="WxNum"></param>
		/// <returns></returns>
		public async Task<dynamic> GetWxNumberDetil(string WxNum)
		{
			if (WxNum != null)
			{
				var query= await _context.lotteryUsers.FindAsync(WxNum);
				if (query == null)
				{
					return ("DuplicateData", "未查到当前用户信息");
				}
				return query;
				
			}
			else
			{
				ModelState.AddModelError("DuplicateData", "该用户微信号未填写");
				return View();
			}
			
		}

		/// <summary>
		/// 获取中奖号码所有数据
		/// </summary>
		/// <returns></returns>
		public async Task<dynamic> GetLootteryTableAsync()
		{
			var lotteryTableList = await _context.lotteryTables.ToListAsync();
			return lotteryTableList;
		}
	}
}