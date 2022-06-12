var bookUID = new URLSearchParams(window.location.search);
bookUID =bookUID.get('BookUID');
var Book;
var jikanNews;

var bxmlHttp = new XMLHttpRequest();
var rxmlHttp = new XMLHttpRequest();
var bookResponse;
var rentingResponse;
var searchBookUrl = 'http://localhost:6160/api/Books/Search?sortBy=BookUID&bookInfo='+bookUID;
var rentingUrl = 'http://localhost:6160/api/RentalHistory/Search?renting=true&bookUID='+bookUID;

rxmlHttp.open("GET",rentingUrl,true);
bxmlHttp.open("GET",searchBookUrl,true);
rxmlHttp.send(null);
bxmlHttp.send(null);

bxmlHttp.onreadystatechange = readyState;
rxmlHttp.onreadystatechange = readyState;

function readyState()
{
    if(bxmlHttp.readyState == 4 && rxmlHttp.readyState == 4)
    {
        if(bxmlHttp.status==200 && rxmlHttp.status==200)
        {
            bookResponse = JSON.parse(bxmlHttp.responseText);
            rentingResponse = JSON.parse(rxmlHttp.responseText);
            //console.log(rentingResponse);
            main();
        }
    }
}

function main()
{
    Book=bookResponse[0];
    var bookName = Book.bookName;
    if(!Book.malID=='')
    {
        jikanMangaAPI();
        jikanNewsAPI();
    }

    document.getElementById('book_Name').innerHTML=bookName;

    bookdata();
}

function jikanMangaAPI()
{
    var jikanManga;
    var jikanMangaUrl= 'https://api.jikan.moe/v4/manga/'+Book.malID;
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET",jikanMangaUrl,true);
    xmlHttp.send(null);
    xmlHttp.onreadystatechange=function () {
        if (xmlHttp.readyState == 4) {
            if (xmlHttp.status == 200) 
            {
                jikanManga = JSON.parse(xmlHttp.responseText);
                var image = jikanManga.data.images.webp.large_image_url;
                document.getElementById('bookimg').setAttribute('src',image);
            }
            else
                alert('Jikan Limit');
        }
    }

}

function jikanNewsAPI()
{
    var jikanNewsUrl= 'https://api.jikan.moe/v4/manga/'+Book.malID+'/news';
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET",jikanNewsUrl,true);
    xmlHttp.send(null);
    xmlHttp.onreadystatechange=function () {
        if (xmlHttp.readyState == 4) {
            if (xmlHttp.status == 200) 
            {
                jikanNews = JSON.parse(xmlHttp.responseText);
                if(!jikanNews.data.length == 0)
                    news();
            }
            else
                alert('Jikan Limit');
        }
    }

}

function bookdata()
{
    Book;
    var bookData=document.getElementById('book_data')
    var late=latecount();
    var inStock=Book.inStock;
    var inRental=Book.inRental;
    var price=Book.price;
    bookData.innerHTML='Late:'+late+' InStock:'+inStock+' InRental:'+inRental+' Price:'+price;

    var table = document.getElementById('renting');
    for(i=0;i<rentingResponse.length;i++)
    {
        var tr=document.createElement('tr');
        table.appendChild(tr);

        var customerNametd = document.createElement('td');
        customerNametd.innerHTML=rentingResponse[i].customerName;
        tr.appendChild(customerNametd);

        var returnDatetd = document.createElement('td');
        returnDatetd.innerHTML=returnshorttime(rentingResponse[i].returnDate);
        tr.appendChild(returnDatetd);
    }
}

function JikanNews()
{

}

function latecount()
{
    var renting = rentingResponse.filter(x=>x.bookUID==bookUID).length;
    var today = new Date();
    var late=0;
    for(i=0;i<rentingResponse.length;i++)
    {
        var returnDate = new Date(rentingResponse[i].returnDate);
        var lateday = today-returnDate;
        lateday = Math.round(lateday/86400000);
        if(lateday>0)
            late++;
    }
    return(late);
}

function news()
{
    for(i=1;i<=5;i++)
    {
        var jikan = jikanNews.data[i-1];
        var newsdiv= document.getElementById('news_'+i);
        newsdiv.innerHTML='';

        var a = document.createElement('a');
        var newstitle = document.createElement('div');
        newstitle.innerHTML = jikan.title;
        a.appendChild(newstitle);
        a.setAttribute('href',jikan.url);
        newsdiv.appendChild(a);

        var newsbody = document.createElement('div');
        newsbody.innerHTML = jikan.excerpt;
        newsdiv.appendChild(newsbody);

        var br = document.createElement('br');
        newsdiv.appendChild(br);

        var newsdate = document.createElement('div');
        newsdate.setAttribute('style','width:100%;text-align:right;');
        newsdate.innerHTML=returnshorttime(jikanNews.data[i-1].date);
        newsdiv.appendChild(newsdate);

    }
}

function returnshorttime(i)
{
    var newsDate = i;
    var date = new Date(newsDate);
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth()+1).padStart(2, '0'); //January is 0!
    var yyyy = date.getFullYear();
    shortdate = yyyy + '-' + mm + '-' + dd;
    return(shortdate);
}