var bookResponse;
var customerResponse;
var historyResponse;
var bookUrl = 'http://localhost:6160/api/Books/Search?sortBy=BookUID';
var customerUrl = 'http://localhost:6160/api/Customers';
var historyUrl = 'http://localhost:6160/api/RentalHistory/Search?renting=true';
var bxmlHttp = new XMLHttpRequest();
var cxmlHttp = new XMLHttpRequest();
var hxmlHttp = new XMLHttpRequest();

bxmlHttp.open("GET",bookUrl,true);
cxmlHttp.open("GET",customerUrl,true);
hxmlHttp.open("GET",historyUrl,true);
bxmlHttp.send(null);
cxmlHttp.send(null);
hxmlHttp.send(null);

bxmlHttp.onreadystatechange = readyState;
cxmlHttp.onreadystatechange = readyState;
hxmlHttp.onreadystatechange = readyState;

var returndate = new Date();
var todaydd = String(returndate.getDate()).padStart(2, '0');
returndate.setDate(returndate.getDate() + 7 )
var returndd = String(returndate.getDate()).padStart(2, '0');
var mm = String(returndate.getMonth()+1).padStart(2, '0'); //January is 0!
var yyyy = returndate.getFullYear();

returndate = yyyy + '-' + mm + '-' + returndd;
var todaydate = yyyy + '-' + mm + '-' + todaydd
document.getElementById('returndate').innerHTML= returndate;

var bookinsertclass = document.getElementsByClassName('bookinsertclass');
for(booki=0; booki < bookinsertclass.length ;booki++)
{
    bookinsertclass[booki].disabled=true;
}
document.getElementById('customerid').disabled=true;


var customerID;
var bookarray =[];
var renttotal = 0;


function readyState()
{
    if(cxmlHttp.readyState==4)
    {
        if(cxmlHttp.status==200)
        {
            customerResponse=JSON.parse(cxmlHttp.responseText);
        }
        else alert('Customer Error');
    }
    if(bxmlHttp.readyState == 4)
    {
        if(bxmlHttp.status==200)
            bookResponse = JSON.parse(bxmlHttp.responseText);
        else alert('Book Error');
    }
    if(hxmlHttp.readyState == 4)
    {
        if(hxmlHttp.status==200)
            historyResponse = JSON.parse(hxmlHttp.responseText);
        else alert('History Error');
    }

}

function customerdropdown()
{
    document.getElementById('resultcustomerdropdown').innerHTML='';
    var searchCustomerName = document.getElementById('customername').value;

    for(i=0;i<customerResponse.length;i++)
    {
        var customerName = customerResponse[i].customerName.toLowerCase();
        if(customerName.includes(searchCustomerName.toLowerCase()))
        {
            var option = document.createElement('option');
            option.innerHTML=customerResponse[i].customerName;
            document.getElementById('resultcustomerdropdown').appendChild(option);
        }
    }
}

function customerselect()
{
    var customerName=document.getElementById('resultcustomerdropdown').value;
    document.getElementById('customername').value=customerName;
    customerID=customerResponse.find(x=>x.customerName == customerName).customerID;
    document.getElementById('customerid').value=customerID;

    for( booki=0 ; booki < bookinsertclass.length ; booki++ )
        bookinsertclass[booki].disabled=false;

}

function bookdropdown()
{
    document.getElementById('resultbookdropdown').innerHTML='';
    var bookSearch=document.getElementById('booksearch').value;
    for(i=0;i<bookResponse.length;i++)
    {
        if(bookResponse[i].bookName.toLowerCase().includes(bookSearch.toLowerCase())||bookResponse[i].bookUID==bookSearch)
        {
            var option = document.createElement('option');
            option.innerHTML=bookResponse[i].bookName;
            document.getElementById('resultbookdropdown').appendChild(option);
        }
    }
}

function bookinsert()
{
    var insertbook = document.getElementById('resultbookdropdown').value;
    var bookinserting = bookResponse.find(x => x.bookName == insertbook);

    var bookinsertlist = document.getElementById('bookinserting');

    if(bookinserting.inStock>0)
    {
        if(!bookarray.includes(bookinserting.bookUID))
        {
            bookarray.push(bookinserting.bookUID);
    
            var tr = document.createElement('tr');
            tr.setAttribute('align','center');
            tr.setAttribute('id',bookinserting.bookName);
            bookinsertlist.appendChild(tr);
    
            var booknametd = document.createElement('td');
            booknametd.innerHTML = bookinserting.bookName;
            tr.appendChild(booknametd);
        
            var bookpricetd = document.createElement('td');
            bookpricetd.innerHTML = bookinserting.price;
            tr.appendChild(bookpricetd);
    
            var cleartd = document.createElement('td');
            cleartd.innerHTML = '<input type="button" value="Clear" onclick=clearbookinserting(this.id) id="'+bookinserting.bookName+'">';
            tr.appendChild(cleartd);

            renttotal += bookinserting.price;
            total();
        }
    }else
        alert('No Book Left');
}

function clearbookinserting(clearid)
{
    var reduceprice = bookResponse.find(x=>x.bookName==clearid).price;
    var clearelement = document.getElementById(clearid);
    clearelement.remove();
    bookarray.splice(bookarray.indexOf(clearid),1)
    renttotal -= reduceprice;
    total();
}

function total()
{
    document.getElementById('total').innerHTML='Total: '+renttotal+' Baht';
}

function rentsubmit()
{
    var customerID = document.getElementById('customerid').value;
    var customerbody = '"customer":{"customerID":"'+customerID+'"},';
    console.log(customerbody);

    for(checker=0;checker<bookarray.length;checker++)
    {
        if(checker==0)
            var bookbody = '"books":[{"bookUID":"'+bookarray[checker]+'"}';
        else
        {
            bookbody += ',{"bookUID":"'+bookarray[checker]+'"}';
        }
    }
    bookbody +='],';

    var returnbody = '"returnDate": "'+todaydate+'"';

    var body = '{'+customerbody+bookbody+returnbody+'}';

    var rentxmlHttp = new XMLHttpRequest();
    rentxmlHttp.open("POST",'http://localhost:6160/api/Customers/Rent',true);
    rentxmlHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    rentxmlHttp.send(body);
    //sleep(200)
    window.location.reload();
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}