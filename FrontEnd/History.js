var historyResponse;
var HistoryUrl = 'http://localhost:6160/api/RentalHistory';
var xmlHttp = new XMLHttpRequest();
xmlHttp.onreadystatechange = readyState;
xmlHttp.open("GET",HistoryUrl,true);
xmlHttp.send(null);
document.getElementById('late').disabled=true;

function readyState()
{
    if(xmlHttp.readyState==4)
    {
        if(xmlHttp.status==200)
        {
            historyResponse=JSON.parse(xmlHttp.responseText);
            for(var i=0;i<historyResponse.length;i++)
                createrow(i);
        }
        else alert('Error');
    }
}

function returnshorttime(i)
{
    var returnDate = historyResponse[i].returnDate;
    var returndate = new Date(returnDate);
    var returndd = String(returndate.getDate()).padStart(2, '0');
    var mm = String(returndate.getMonth()+1).padStart(2, '0'); //January is 0!
    var yyyy = returndate.getFullYear();
    returndate = yyyy + '-' + mm + '-' + returndd;
    return(returndate);
}

function createrow(i)
{
    var tr = document.createElement('tr');
    if( i%2 == 0 )
        tr.setAttribute('style','background-color: rgb(240,240,240);');
    document.getElementById('list').appendChild(tr);

    var idtd = document.createElement('td');
    idtd.innerHTML=historyResponse[i].customerID;
    tr.appendChild(idtd);

    var datetd = document.createElement('td');
    datetd.innerText=historyResponse[i].rentalDate;
    tr.appendChild(datetd);

    var nametd = document.createElement('td');
    nametd.innerHTML=historyResponse[i].customerName;
    tr.appendChild(nametd);

    var bookNametd = document.createElement('td');
    bookNametd.innerHTML=historyResponse[i].bookName;
    tr.appendChild(bookNametd);

    var pricetd = document.createElement('td');
    pricetd.innerHTML=historyResponse[i].price;
    tr.appendChild(pricetd);

    var returnDatetd = document.createElement('td');
    returnDatetd.innerHTML=returnshorttime(i);
    tr.appendChild(returnDatetd);

    var returntd = document.createElement('td');
    returntd.innerHTML=historyResponse[i].return;
    tr.appendChild(returntd);

    /*var infotd = document.createElement('td');
    infotd.innerHTML= 'Info';
    tr.appendChild(infotd);*/
}

// var wage = document.getElementById("searchhistory");
// wage.addEventListener("change", searchHistory);  //Always call function searchHistory when finish typing
function searchHistory()
{
    var id=document.getElementById('searchhistory').value;
    var renting = document.querySelector('#renting').checked;
    var late = document.querySelector('#late').checked;
    var searchHistoryUrl = 'http://localhost:6160/api/RentalHistory/Search?renting='+renting+'&late='+late+'&id='+id;
    if (renting == false)
        searchHistoryUrl = 'http://localhost:6160/api/RentalHistory/Search?&id='+id;
    document.getElementById('list').innerHTML='<tr><th>C.ID</th><th>Rent Date</th><th>Name</th><th>Book</th><th>Price</th><th>Return Date</th><th>Return</th></tr>';
    xmlHttp.onreadystatechange = readyState;
    xmlHttp.open("GET",searchHistoryUrl,true);
    xmlHttp.send(null);
}

function rentingclicked()
{
    var renting = document.querySelector('#renting').checked;
    if(renting)
    {
        document.getElementById('late').disabled=false;
    }else
    {
        document.getElementById('late').disabled=true;
        document.getElementById('late').value=false;
    }
}