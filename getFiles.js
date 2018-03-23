var fs = require('fs');
var cheerio = require('cheerio');
// Array.prototype.sortBy = function(p) {
//   return this.slice(0).sort(function(a,b) {
//     return (a[p] > b[p]) ? 1 : (a[p] < b[p]) ? -1 : 0;
//   });
// }
compare = function(x, y) {
  return x.match(/\d+/g)[0] - y.match(/\d+/g)[0];
}

// var hymn = {'id':'', 'link':'', 'title':''};
var hymns = [];

var content = '';
var files = [];
var dir = 'C:\\_Projects\\tca\\hymn\\test\\';
fs.readdirSync(dir).forEach(function(item) {
    if(item.match("^hymn.[0-9]*\.html$")){
      files.push(item);
    }
});
files.sort(compare);

for(var index in files){
    var html = fs.readFileSync(dir + files[index], 'utf8');
    var $ = cheerio.load(html, { decodeEntities: false });
    // $("title").text($("title").text().toUpperCase().replace("  ", " "));
    // fs.writeFileSync('C:\\_Projects\\tca\\hymn\\test\\' + files[index], $("html").html(), 'utf8');

    content = content + '<div class="hymnTitle"><a href="../hymn/' + files[index] + '">' + $("title").text() + '</a></div>\n';

  }

fs.writeFile('C:\\_Projects\\tca\\hymn\\tableOfContent.html', content, function(err){
   if(err) throw err;
   console.log('Saved');
 })
// console.log(content);
          // var hymn = {'id':'', 'link':'', 'title':''};
//           // hymn['link'] =  items[i];
//           // console.log(items[i]);
//           var fn = 'C:\\_Projects\\tca\\hymn\\' + items[i];
//           fs.readFile(fn,  'utf8', function(err, html){
//             console.log(fn);
//             if(err){
//               console.log(err);
//             }
//             else {
//               var $ = cheerio.load(html);
//               var title = $("title").text();
//               // hymn = {'id': title.split(".", 1)[0] , 'link': file, 'title': title};
//               hymn['id'] = title.split(".", 1)[0];
//               hymn['title'] = title;
//               // hymn['link'] =  _filename;
//               // console.log(hymn);
//               // hymns.push(hymn);
//               // fs.writeFile('C:\\_Projects\\tca\\hymn\\tableOfContent.html', '<div class="hymnTitle"><a href="../hymn/"' + items[i] + '>' + $("title").text() + '</a></div>', function(err){
//               //   if(err) throw err;
//               //   console.log('Saved');
//               // })
//
//               // console.log($("title").text());
//             }
//           });
//         }
//     }
// });
