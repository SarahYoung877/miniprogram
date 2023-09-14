using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace aspnetapp
{
    public class Counter
    {
        public int id { get; set; }
        public int count { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    #region 实体Model
    /// <summary>
    /// 今日概况
    /// </summary>
    [Keyless]
    public class Data_Day
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public int line1 { get; set; } = 0;
        public int line2 { get; set; } = 0;
        public int line3 { get; set; } = 0;
        public int total { get; set; } = 0;
        public int tiaoshi { get; set; } = 0;
        public int jiaofu { get; set; } = 0;
    }

    /// <summary>
    /// 本月累计
    /// </summary>
    [Keyless]
    public class Data_Month
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public int shengchan { get; set; } = 0;
        public int tiaoshi { get; set; } = 0;
        public int jiaofu { get; set; } = 0;
    }

    [Keyless]
    public class Data_OutHourly
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public string time { get; set; }
        public int qty { get; set; } = 0;
    }

    /// <summary>
    /// 近7天
    /// </summary>
    [Keyless]
    public class Data_7Day
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public string time { get; set; }
        public int shengchan { get; set; } = 0;
        public int tiaoshi { get; set; } = 0;
        public int jiaofu { get; set; } = 0;
    }

    /// <summary>
    /// 立库库存
    /// </summary>
    [Keyless]
    public class Data_Inv
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public string datatype { get; set; }
        public int jizuo { get; set; } = 0;
        public int yeyinlun { get; set; } = 0;
        public int zhuanzi { get; set; } = 0;
    }
    /// <summary>
    /// 立库进出
    /// </summary>
    [Keyless]
    public class Data_InOut
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public string datatype { get; set; }
        public int jizuo { get; set; } = 0;
        public int yeyinlun { get; set; } = 0;
        public int zhuanzi { get; set; } = 0;
    }

    /// <summary>
    /// 立库每小时进出
    /// </summary>
    [Keyless]
    public class Data_InOutHourly
    {
        /// <summary>
        /// 产线，也就是menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime date { get; set; }

        public string time { get; set; }
        public int inData { get; set; } = 0;
        public int outData { get; set; } = 0;
    }


    [Keyless]
    public class Menus
    {
        public string code { get; set; }

        public string name { get; set; }
    }

    public class Users
    {
        [Key]
        public string openid { get; set; }

        public string name { get; set; }

        public string active { get; set; }

        public string admin { get; set; }
    }

    public class UserMenus
    {
        public string openid { get; set; }
        public string menucode { get; set; }
    }

    #endregion

    #region Dto
    /// <summary>
    /// 保存用户权限
    /// </summary>
    public class SaveUserMenu
    {
        public string openid { get; set; }

        public List<string> codeList { get; set; }
    }


    public class OutHourlyDto
    {
        public string[] xData { get; set; }

        public int[] yData { get; set; }
    }
    public class Day7Dto
    {
        public string[] xData { get; set; }
        public int[] shengchan { get; set; }
        public int[] tiaoshi { get; set; }
        public int[] jiaofu { get; set; }
    }
    public class InvDto
    {
        public int jizuo { get; set; } = 0;
        public int yeyinlun { get; set; } = 0;
        public int zhuanzi { get; set; } = 0;

        public int totaljz { get; set; } = 0;
        public int totaly { get; set; } = 0;
        public int totalz { get; set; } = 0;
    }

    public class InOutDto
    {
        public int[] inData { get; set; }
        public int[] outData { get; set; }
    }

    public class InOutHourlyDto
    {
        public string[] xData { get; set; }
        public int[] inData { get; set; }
        public int[] outData { get; set; }
    }

    public class Result<T>
    {
        public string code { get; set; } = "0";

        public string message { get; set; }

        public T data { get; set; }
    }
    #endregion
}