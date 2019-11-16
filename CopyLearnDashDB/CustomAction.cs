using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace CopyLearnDashDB
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            session.Log("Begin CustomAction1");
            string ApplicationPath = session.CustomActionData.ToString();
            File.Copy("");
            return ActionResult.Success;
        }
    }
}
