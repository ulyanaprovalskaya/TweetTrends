using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using TweetTrends.Business_Layer;
using TweetTrends.Data_Access_Layer;

namespace TweetTrends.Service_Layer
{
    class Service
    {
        StateDao stateDao;

        public Service(string tweetsFilepath)
        {
            stateDao = new StateDao(tweetsFilepath);
        }

        public Dictionary<string, State> GetStates()
        {
            return stateDao.GetStates();
        }

        public List<GMapPolygon> GetPolygons()
        {
            return stateDao.GetPolygons();
        }


    }
}
