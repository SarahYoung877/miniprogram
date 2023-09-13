using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#region
public class CounterRequest
{
    public string action { get; set; }
}
public class CounterResponse
{
    public int data { get; set; }
}

public class DataRequest
{
    public string menucode { get; set; }
}
#endregion

namespace aspnetapp.Controllers
{
    [Route("api/count")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly CounterContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CounterController(CounterContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Counter
        private async Task<Counter> getCounterWithInit()
        {
            var counters = await _context.Counters.ToListAsync();
            if (counters.Count() > 0)
            {
                return counters[0];
            }
            else
            {
                var counter = new Counter { count = 0, createdAt = DateTime.Now, updatedAt = DateTime.Now };
                _context.Counters.Add(counter);
                await _context.SaveChangesAsync();
                return counter;
            }
        }
        // GET: api/count
        [HttpGet]
        public async Task<ActionResult<CounterResponse>> GetCounter()
        {
            var counter = await getCounterWithInit();
            return new CounterResponse { data = counter.count };
        }

        // POST: api/Counter
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CounterResponse>> PostCounter(CounterRequest data)
        {
            if (data.action == "inc")
            {
                var counter = await getCounterWithInit();
                counter.count += 1;
                counter.updatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return new CounterResponse { data = counter.count };
            }
            else if (data.action == "clear")
            {
                var counter = await getCounterWithInit();
                counter.count = 0;
                counter.updatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return new CounterResponse { data = counter.count };
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region 权限

        [HttpGet]
        public async Task<ActionResult<Result<List<Menus>>>> GetUserMenus()
        {
            var result = new Result<List<Menus>>();
            try
            {
                // 从header中取到openid
                var headers = _httpContextAccessor.HttpContext.Request.Headers;
                var openid = headers["X-WX-OPENID"];

                // 用户有无权限
                var user = await _context.Users.Where(u => u.openid == openid).FirstOrDefaultAsync();
                if (user == null || string.IsNullOrEmpty(user.openid))
                {
                    result.code = "2";
                    result.message = "用户未申请权限";
                    return result;
                }

                if (user.active == "0")
                {
                    result.code = "3";
                    result.message = "审核未通过";
                    return result;
                }

                // 权限
                var menus = await (from m in _context.Menus
                                   join um in _context.UserMenus on m.code equals um.menucode
                                   where um.openid == openid
                                   select new Menus { code = m.code, name = m.name }
                                   ).ToListAsync();

                result.code = "1";
                result.message = "OK";
                result.data = menus;
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Result<string>>> ApplyUser(CounterRequest request)
        {
            var result = new Result<string>();
            try
            {
                // 从header中取到openid
                var headers = _httpContextAccessor.HttpContext.Request.Headers;
                var openid = headers["X-WX-OPENID"];

                var user = await _context.Users.Where(u => u.openid == openid).FirstOrDefaultAsync();
                if (user == null || user.openid == "")
                {
                    var newUser = new Users { openid = openid, name = request.action, active = "0", admin = "0" };
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    result.code = "1";
                }
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 申请通过
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Result<string>>> SaveUserMenu(SaveUserMenu menu)
        {
            var result = new Result<string>();

            try
            {
                var oldData = await _context.UserMenus.Where(u => u.openid == menu.openid).ToListAsync();
                if (oldData != null && oldData.Count > 0)
                {
                    _context.UserMenus.RemoveRange(oldData);

                    await _context.SaveChangesAsync();
                }

                var data = menu.codeList.Select(c => new UserMenus { openid = menu.openid, menucode = c }).ToList();

                await _context.UserMenus.AddRangeAsync(data);

                result.code = "1";
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
            }
            return result;
        }

        #endregion

        #region 数据
        /// <summary>
        /// 今日概况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<Data_Day>>> GetDataDay(DataRequest request)
        {
            var data = await _context.Data_Day.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstOrDefaultAsync();
            return new Result<Data_Day> { code = "1", data = data };
        }

        /// <summary>
        /// 本月累计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<Data_Month>>> GetDataMonth(DataRequest request)
        {
            var data = await _context.Data_Month.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstOrDefaultAsync();
            return new Result<Data_Month> { code = "1", data = data };
        }

        /// <summary>
        /// 每小时产出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<OutHourlyDto>>> GetDataOutHourly(DataRequest request)
        {
            var maxDate = await _context.Data_OutHourly.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstAsync();

            var data = await _context.Data_OutHourly.Where(d => d.menucode == request.menucode && d.date == maxDate.date).OrderBy(d => d.time).ToListAsync();

            var result = new OutHourlyDto();
            result.xData = data.Select(d => d.time).ToArray();
            result.yData = data.Select(d => d.qty).ToArray();
            return new Result<OutHourlyDto> { code = "1", data = result };
        }

        /// <summary>
        /// 近7天
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<Day7Dto>>> GetData7Day(DataRequest request)
        {
            var maxDate = await _context.Data_7Day.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstAsync();

            var data = await _context.Data_7Day.Where(d => d.menucode == request.menucode && d.date == maxDate.date).OrderBy(d => d.time).ToListAsync();

            var result = new Day7Dto();
            result.xData = data.Select(d => d.time).ToArray();
            result.shengchan = data.Select(d => d.shengchan).ToArray();
            result.tiaoshi = data.Select(d => d.tiaoshi).ToArray();
            result.jiaofu = data.Select(d => d.jiaofu).ToArray();
            return new Result<Day7Dto> { code = "1", data = result };
        }

        /// <summary>
        /// 立库库存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<InvDto>>> GetDataInv(DataRequest request)
        {
            var maxDate = await _context.Data_Inv.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstAsync();

            var data = await _context.Data_Inv.Where(d => d.menucode == request.menucode && d.date == maxDate.date).ToListAsync();

            var cnt = data.Where(d => d.datatype == "qty").FirstOrDefault() ?? new Data_Inv();
            var total = data.Where(d => d.datatype == "total").FirstOrDefault() ?? new Data_Inv();

            var result = new InvDto();
            result.jizuo = cnt.jizuo;
            result.yeyinlun = cnt.yeyinlun;
            result.zhuanzi = cnt.zhuanzi;
            result.totaljz = total.jizuo;
            result.totaly = total.yeyinlun;
            result.totalz = total.zhuanzi;
            return new Result<InvDto> { code = "1", data = result };
        }

        /// <summary>
        /// 立库进出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<InOutDto>>> GetDataInOut(DataRequest request)
        {
            var maxDate = await _context.Data_InOut.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstAsync();

            var data = await _context.Data_InOut.Where(d => d.menucode == request.menucode && d.date == maxDate.date).ToListAsync();

            var inData = data.Where(d => d.datatype == "inData").FirstOrDefault() ?? new Data_InOut();
            var outData = data.Where(d => d.datatype == "outData").FirstOrDefault() ?? new Data_InOut();

            var result = new InOutDto();
            result.inData = new int[3] { inData.jizuo, inData.yeyinlun, inData.zhuanzi };
            result.outData = new int[3] { outData.jizuo, outData.yeyinlun, outData.zhuanzi };
            return new Result<InOutDto> { code = "1", data = result };
        }

        /// <summary>
        /// 立库每小时进出效率
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Result<InOutHourlyDto>>> GetDataInOutHourly(DataRequest request)
        {
            var maxDate = await _context.Data_InOutHourly.Where(d => d.menucode == request.menucode).OrderByDescending(d => d.date).FirstAsync();

            var data = await _context.Data_InOutHourly.Where(d => d.menucode == request.menucode && d.date == maxDate.date).OrderBy(d => d.time).ToListAsync();

            var result = new InOutHourlyDto();
            result.xData = data.Select(d => d.time).ToArray();
            result.inData = data.Select(d => d.inData).ToArray();
            result.outData = data.Select(d => d.outData).ToArray();
            return new Result<InOutHourlyDto> { code = "1", data = result };
        }
        #endregion
    }
}
