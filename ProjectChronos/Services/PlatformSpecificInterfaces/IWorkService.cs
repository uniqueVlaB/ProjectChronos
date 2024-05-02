using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.Services.PlatformSpecificInterfaces
{
    public interface IWorkService
    {
        public Task<bool> StartDailyWork();
        public Task<bool> StopDailyWork();
    }
}
