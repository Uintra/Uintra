﻿using System;
using System.Collections.Generic;
using Uintra.Core.Updater._2._0.Steps;

namespace Uintra.Core.Updater._2._0 
{
    public class Migration : IMigration
    {
        public Version Version => new Version("2.0");
        
        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return new SetupDefaultMemberGroupsStep();
                yield return new SetupDefaultMemberGroupsPermissionsStep();
                yield return new CreateDefaultMemberStep();
                yield return new SetupDefaultMediaFoldersStep();
                yield return new ExtendDefaultAdminRightsStep();
            }
        }
    }
}