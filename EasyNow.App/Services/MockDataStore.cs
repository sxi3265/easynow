using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNow.App.Models;

namespace EasyNow.App.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item
                {
                    Id = Guid.NewGuid().ToString(), Text = "咪咕", Description="This is an item description.",
                    Source = @"app.startAuto();
                app.launchApp('com.ophone.reader.ui');
                app.toast('等待首页搜索');
                app.toast('包名:'+ui.currentPackageName+' Activity:'+ui.currentActivity);
                var node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/btn_bookshelf_search'||node.id=='com.ophone.reader.ui:id/recom_btn_search';});
                node.click();
                app.toast('等待输入框');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/etSearch';});
                node.inputText('天天爱阅读');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/btn_search_txt';});
                node.click();
                app.toast('等待搜索结果');
                node=ui.wait(function(node){return node.text=='%E6%90%9C%E7%B4%A2%E5%8F%A3%E4%BB%A4%E5%9B%BE';});
                node.click();
                node=ui.wait(function(node){return node.text=='去阅读'||node.text=='已完成';});
                if(node.text=='已完成'){
                    if(ui.where(function(node){return node.text=='已完成';}).length==2){
                        app.toast('已完成打卡');
                        return;
                    }
                    app.toast('已完成阅读，准备开始签到');
                    node=ui.wait(function(node){return node.text=='签到'&&node.clickable;});
                    node.click();
                    return;
                }
                node.click();
                node=ui.wait(function(node){return node.text=='cover180240';});
                node.click();
                app.toast('进入书籍');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/reader_content_view';});
                app.toast('开始翻页');
                var startTime = new Date().getTime();
                while(true){
                    var rnd = Math.random();
                    device.touch(node.boundsInScreen.left+node.boundsInScreen.width()*(0.80+0.5*rnd),node.boundsInScreen.top+node.boundsInScreen.height()*(0.80+0.5*rnd));
                    app.sleep(10000);
                    if(new Date().getTime()-startTime>1020000){
                        app.toast('已阅读17分钟');
                        return;
                    }
                }
                "
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(), Text = "什么值得买", Description="This is an item description.",
                    Source = @"app.startAuto();
                    app.launchApp('com.smzdm.client.android');
                    var node=ui.wait(function(n){return n.id=='com.smzdm.client.android:id/tab_usercenter';});
                    node.click();
                    node = ui.wait(function(n){return n.id=='com.smzdm.client.android:id/tv_login_sign';});
                    if(node.text=='签到'){
                        node.click();
                        app.toast('完成签到');
                    }else{
                        app.toast(node.text);
                    }
                    "
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(), Text = "截图测试", Description="This is an item description.",
                    Source = @"ui.captureScreen();"
                },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}