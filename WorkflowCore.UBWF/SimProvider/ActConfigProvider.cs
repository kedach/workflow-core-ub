﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.SimProvider
{
    public class ActLineConfigProvider
    {
        Dictionary<string, string> configCache = new Dictionary<string, string>();
        static ActLineConfigProvider m_Instance = null;
        private ActLineConfigProvider()
        { }
        static readonly object locker = new object();
        public static ActLineConfigProvider Instance
        {
            get
            {
                lock (locker)
                {
                    if (m_Instance == null)
                        m_Instance = new ActLineConfigProvider();
                }
                return m_Instance;
            }
        }

        public string Get(string actId)
        {
            if (configCache.ContainsKey(actId))
                return configCache[actId];
            //从数据库中提取
            string actConfig = "";
            configCache.Add(actId, actConfig);
            return actConfig;
        }
    }
}
