﻿using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.UBWF.Models.TOA;

namespace WorkflowCore.UBWF.Models
{
    /// <summary>
    /// 上下文是根据step运转实时变化的。
    /// GetProfile可以获取当前Scope的运行镜像。
    /// 几个固定的上下文LastJobId
    /// </summary>
    public class ProcInstContext
    {
        public string ProcInstId { get; set; }
        /// <summary>
        /// 节点的ID
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 保存各个节点的上下文信息
        /// ISTOA  LastJobID CurJobs OutCome Destionation Destination
        /// </summary>
        public IDictionary<string, IDictionary<string, object>> scops = new Dictionary<string, IDictionary<string, object>>();

        public bool ExistScope(string scope) 
        {
            return scops.ContainsKey(scope);
        }


        public object GetValue(string key)
        {
            if (scops.ContainsKey(Scope) && scops[Scope].ContainsKey(key))
            {
                return scops[Scope][key];
            }
            return null;
        }
        public void SetValue(string key, object value)
        {
            if (!scops.ContainsKey(Scope))
            {
                scops.Add(Scope, new Dictionary<string, object>());
            }

            if (!scops[Scope].ContainsKey(key))
            {
                scops[Scope].Add(key, value);
            }
            else
            {
                scops[Scope][key] = value;
            }
        }
        /// <summary>
        /// 返回当前Scope的镜像
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetProfile()
        {
            IDictionary<string, object> profile = scops[Scope];
            IDictionary<string, object> profileImage = new Dictionary<string, object>();
            foreach (var p in profile)
            {
                profileImage.Add(p.Key, p.Value);
            }
            return profileImage;
        }

        public void ClearScopeContext()
        {
            //是否要记录历史 需要则持久化。
            IDictionary<string, object> scopeContext = scops[Scope];
            scopeContext.Clear();
        }
    }
}
