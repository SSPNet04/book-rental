//console.log('Javascript Link');
var bookResponse;
var searchBookUrl = 'http://localhost:6160/api/Books/Search?sortBy=BookUID&descending=false&name=';
var bxmlHttp = new XMLHttpRequest();
bxmlHttp.open("GET",searchBookUrl,true);
bxmlHttp.send(null);

var historyResponse;
var HistoryUrl = 'http://localhost:6160/api/RentalHistory/Search?renting=true&late=true';
var hxmlHttp = new XMLHttpRequest();
hxmlHttp.open("GET",HistoryUrl,true);
hxmlHttp.send(null);

bxmlHttp.onreadystatechange = readyState;
hxmlHttp.onreadystatechange = readyState;

function readyState()
{
    //console.log(xmlHttp.readyState);
    if(hxmlHttp.readyState==4)
    {
        if(hxmlHttp.status==200)
        {
            historyResponse=JSON.parse(hxmlHttp.responseText);
        }
        else alert('History Error');
    }
    if(bxmlHttp.readyState == 4 && hxmlHttp.readyState == 4)
    {
        if(bxmlHttp.status==200)
        {
            var bookResponseStr = bxmlHttp.responseText;
            //console.log(bookResponseStr);
            bookResponse = JSON.parse(bookResponseStr);
            for(var i=0;i<bookResponse.length;i++)
            {
                //console.log('loop'+i);
                //console.log(bookResponse[i].bookName);
                createrow(i);
            }
            //console.log(bookResponseStr);
        }
        else alert('Book Error');
    }

}


function createrow(i)
{

    var tr =document.createElement('tr');
    document.getElementById('list').appendChild(tr);

    var name_a = document.createElement("a");
    var nametd = document.createElement("td");
    name_a.setAttribute('href','BookInfo.html?BookUID='+bookResponse[i].bookUID);
    name_a.innerHTML = bookResponse[i].bookName;
    nametd.appendChild(name_a);
    tr.appendChild(nametd);

    var uidtd = document.createElement("td");
    uidtd.innerHTML = bookResponse[i].bookUID;
    if( i%2 == 0 )
        tr.setAttribute('style','background-color: rgb(240,240,240);');
    tr.appendChild(uidtd);

    var malidtd = document.createElement("td");
    if(bookResponse[i].malID == '')
        malidtd.innerHTML = '-'
    else
        malidtd.innerHTML = bookResponse[i].malID;
    tr.appendChild(malidtd);

    var latetd = document.createElement("td");
    latetd.innerHTML = latecount(i);
    tr.appendChild(latetd);

    var stocktd = document.createElement("td");
    stocktd.innerHTML = bookResponse[i].inStock;
    tr.appendChild(stocktd);

    var rentaltd = document.createElement("td");
    rentaltd.innerHTML = bookResponse[i].inRental;
    tr.appendChild(rentaltd);

    var pricetd = document.createElement("td");
    pricetd.innerHTML = bookResponse[i].price;
    tr.appendChild(pricetd);

    var atrtd = document.createElement("td");
    atrtd.innerHTML = bookResponse[i].allTimeRent;
    tr.appendChild(atrtd);

    var edittd = document.createElement("td");
    edittd.innerHTML = '<input type="button" value="Edit" onclick=edit(this.id) id="'+bookResponse[i].bookUID+'">';
    tr.appendChild(edittd);
}

function create()
{
    var postHttp = new XMLHttpRequest();
    var newBook;
    var bookUID = document.getElementById('bookuid').value;
    var bookName = document.getElementById('bookname').value;
    var malID = document.getElementById('malid').value;
    var inStock = document.getElementById('instock').value;
    var price = document.getElementById('price').value;
    newBook ='{"bookUID":"'+ bookUID + '","malID":"'+malID+'","bookName":"'+bookName+'","inStock":'+inStock+',"price":'+price+'}';
    var bookobj = JSON.parse(newBook);
    var postBookUrl = 'http://localhost:6160/api/Books';
    if(bookUID == '')
    {
        alert('ID Field Is Required');
        return;
    }
    postHttp.onreadystatechange = function()
    {
        if (postHttp.readyState == 4)
        {
            if(postHttp.response == 'Duplicate UID')
                alert('Duplicate UID');
            else
                window.location.reload();
        }

    }
    postHttp.open("POST",postBookUrl,true);
    postHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    postHttp.send(newBook);
}

function sortdata()
{
    var sortBookName = document.getElementById('search').value;
    var sortBy = document.getElementById('sortby').value;
    var descending = document.querySelector('#descending');
    var check = descending.checked;
    //console.log(check);
    var sortBookUrl = 'http://localhost:6160/api/Books/Search?sortBy='+sortBy+'&descending='+check+'&name='+sortBookName;
    //console.log(sortBookUrl);
    document.getElementById('list').innerHTML='<tr><th width="200">Book</th><th>BookUID</th><th>Late</th><th>In Stock</th><th>In Rental</th><th>Price</th><th width ="150">All Time Rent</th></tr>';
    bxmlHttp.onreadystatechange = readyState;
    bxmlHttp.open("GET",sortBookUrl,true);
    bxmlHttp.send(null);
}

function latecount(i)
{
    var bookUID = bookResponse[i].bookUID;
    var latebook = historyResponse.filter(x=>x.bookUID==bookUID).length;
    console.log(latebook);
    if(latebook > 0)
    {
        return(latebook);
    }
    else
        return (0);
    // console.log(bookUID,late);

}