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

    #region ʵ��Model
    /// <summary>
    /// ���ոſ�
    /// </summary>
    [Keyless]
    public class Data_Day
    {
        /// <summary>
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
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
    /// �����ۼ�
    /// </summary>
    [Keyless]
    public class Data_Month
    {
        /// <summary>
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
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
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime date { get; set; }

        public string time { get; set; }
        public int qty { get; set; } = 0;
    }

    /// <summary>
    /// ��7��
    /// </summary>
    [Keyless]
    public class Data_7Day
    {
        /// <summary>
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime date { get; set; }

        public string time { get; set; }
        public int shengchan { get; set; } = 0;
        public int tiaoshi { get; set; } = 0;
        public int jiaofu { get; set; } = 0;
    }

    /// <summary>
    /// ������
    /// </summary>
    [Keyless]
    public class Data_Inv
    {
        /// <summary>
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime date { get; set; }

        public string datatype { get; set; }
        public int jizuo { get; set; } = 0;
        public int yeyinlun { get; set; } = 0;
        public int zhuanzi { get; set; } = 0;
    }
    /// <summary>
    /// �������
    /// </summary>
    [Keyless]
    public class Data_InOut
    {
        /// <summary>
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime date { get; set; }

        public string datatype { get; set; }
        public int jizuo { get; set; } = 0;
        public int yeyinlun { get; set; } = 0;
        public int zhuanzi { get; set; } = 0;
    }

    /// <summary>
    /// ����ÿСʱ����
    /// </summary>
    [Keyless]
    public class Data_InOutHourly
    {
        /// <summary>
        /// ���ߣ�Ҳ����menu code
        /// </summary>
        public string menucode { get; set; }

        /// <summary>
        /// ����ʱ��
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
    /// �����û�Ȩ��
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