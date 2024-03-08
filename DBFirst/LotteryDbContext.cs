using Lottery.Models;
using Microsoft.EntityFrameworkCore;

namespace Lottery.DBFirst
{
	public class LotteryDbContext :DbContext
	{
		public DbSet<LotteryTable> lotteryTables { get; set; }

		public DbSet<LotteryUser> lotteryUsers { get; set; }

		// 可选：覆盖 OnConfiguring 方法，配置数据库连接字符串
		public LotteryDbContext(DbContextOptions<LotteryDbContext> options) : base(options)
		{
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("YourConnectionString");//把字符串换成自己本地数据库的字符串
		//	//sqlserver 字符串连接
		//	//Server = YourServerAddress; Database = YourDatabaseName; User Id = YourUsername; Password = YourPassword;
		//	//MySQL 连接字符串格式：
		//	//Server=YourServerAddress;Database=YourDatabaseName;Uid=YourUsername;Pwd=YourPassword;
		//}
	}
}
