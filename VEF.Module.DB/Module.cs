﻿using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Interface;

namespace VEF.Module.DB
{
    [Export( typeof(IModule))]
    public class Module : IModule
    {
        //[ImportingConstructor]
        public Module()
        {
           
        }
        
        public void PreInitialize()
        {

        }

        public void Initialize()
        {

        }

        public void PostInitialize()
        {

        }

    }

}
