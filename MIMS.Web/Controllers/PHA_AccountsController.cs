﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIMS.IBusiness;
using MIMS.Business;
using MIMS.Entity;
using Newtonsoft.Json;
using System.Collections;
using MIMS.Entity.Model;

namespace MIMS.Web.Controllers
{
    public class PHA_AccountsController : Controller
    {
        IPHA_AccountsBLL ipha_accountsbll = new PHA_AccountsBLL();
        IPSS_PurchaseCompanyBLL ipss_purchasecompanybll = new PSS_PurchaseCompanyBLL();


        // GET: PHA_Accounts
        [RoleActionFilter]
        public ActionResult PhaAccounts()
        {
            return View();
        }

        /// <summary>
        /// datagrid数据绑定
        /// </summary>
        /// <param name="rows">每行显示的条数</param>
        /// <param name="page">当前页</param>
        /// <param name="sort">排序字段</param>
        /// <param name="order">排序方式</param>
        /// <param name="query">条件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadList(string rows, string page, string sort, string order, string pinyin, string companyID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("PinyinCode",pinyin);
            ht.Add("CompanyID",companyID);
            int count = default(int);
            IList list = ipha_accountsbll.GetPageList(ht, sort, order, int.Parse(page), int.Parse(rows), ref count);
            return Content(JsonConvert.SerializeObject(new
            {
                total = count,
                rows = list
            }));
        }

        [HttpPost]
        public ActionResult LoadForm(string phaCode, string orginID)
        {
            return Json(ipha_accountsbll.GetEntity(phaCode, orginID));
        }


        [HttpPost]
        public ActionResult LoadSelectComPany()
        {
            return Json(ipss_purchasecompanybll.GetList());
        }

        [HttpPost]
        public ActionResult AcceptClick(PHA_Accounts obj)
        {
            string key = Request["key"];
            int isOk = default(int);
            //key表示是否编辑的标识，1表示处于编辑状态 0表示增加状态
            if (key == "1")
                isOk = ipha_accountsbll.Update(obj);
            else
            {
                PHA_Accounts temp = ipha_accountsbll.GetEntity(obj.PhaCode, obj.OrginID.ToString());
                if (temp == null)
                    isOk = ipha_accountsbll.Insert(obj);
                else
                    isOk = -1;         //存在相同的记录，出错。
            }
            return Content(isOk.ToString());
        }


        [HttpPost]
        public ActionResult Del(string phaCode, string orginID)
        {
            return Content(ipha_accountsbll.Delete(new PHA_Accounts
            {
                PhaCode = phaCode,
                OrginID = int.Parse(orginID)
            }).ToString());
        }

    }
}