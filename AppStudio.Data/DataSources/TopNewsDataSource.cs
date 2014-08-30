using System;
using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.Data
{
    public class TopNewsDataSource : IDataSource<TopNewsSchema>
    {
        private const string _appId = "a3e9917b-322a-42ab-b606-ac8341197c4a";
        private const string _dataSourceName = "20aa14cf-bcfb-4b43-83a8-96c745b7bdfb";

        private IEnumerable<TopNewsSchema> _data = null;

        public TopNewsDataSource()
        {
        }

        public async Task<IEnumerable<TopNewsSchema>> LoadData()
        {
            if (_data == null)
            {
                try
                {
                    var serviceDataProvider = new ServiceDataProvider(_appId, _dataSourceName);
                    _data = await serviceDataProvider.Load<TopNewsSchema>();
                }
                catch (Exception ex)
                {
                    AppLogs.WriteError("TopNewsDataSource.LoadData", ex.ToString());
                }
            }
            return _data;
        }

        public async Task<IEnumerable<TopNewsSchema>> Refresh()
        {
            _data = null;
            return await LoadData();
        }
    }
}
