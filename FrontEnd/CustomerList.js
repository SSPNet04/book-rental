var customerResponse;
var customerUrl = 'http://localhost:6160/api/Customers';
var xmlHttp = new XMLHttpRequest();
xmlHttp.onreadystatechange = readyState;
xmlHttp.open("GET",customerUrl,true);
xmlHttp.send(null);

function readyState()
{
    if(xmlHttp.readyState==4)
    {
        if(xmlHttp.status==200)
        {
            customerResponse=JSON.parse(xmlHttp.responseText);
            document.getElementById('list').innerHTML='<tr><th>ID</th><th>Name</th><th>Phone Number</th><th>Point</th><th>All time spent</th><th>Book Renting</th></tr>';
            for(var i=0;i<customerResponse.length;i++)
                createrow(i);
        }
        else alert('Error');
    }
}

function createrow(i)
{
    var tr = document.createElement('tr');
    if( i%2 == 0 )
        tr.setAttribute('style','background-color: rgb(240,240,240);');
    document.getElementById('list').appendChild(tr);

    var idtd = document.createElement('td');
    idtd.innerHTML=customerResponse[i].customerID;
    tr.appendChild(idtd);

    var nametd = document.createElement('td');
    nametd.innerText=customerResponse[i].customerName;
    tr.appendChild(nametd);

    var phonetd = document.createElement('td');
    phonetd.innerHTML=customerResponse[i].phoneNumber;
    tr.appendChild(phonetd);

    var pointtd = document.createElement('td');
    pointtd.innerHTML=customerResponse[i].point;
    tr.appendChild(pointtd);

    var atstd = document.createElement('td');
    atstd.innerHTML=customerResponse[i].allTimeSpent;
    tr.appendChild(atstd);

    var rentingtd = document.createElement('td');
    rentingtd.innerHTML=customerResponse[i].bookRenting;
    tr.appendChild(rentingtd);

    var infotd = document.createElement('td');
    infotd.innerHTML= '<input type="button" value="Info" onclick=info(this.id),edit(this.id) id="'+customerResponse[i].customerID+'">';
    tr.appendChild(infotd);
}

function sortdata()
{
    var keyword=document.getElementById('search').value;
    var sortBy=document.getElementById('sortby').value;
    var descending=document.querySelector('#descending');
    descending = descending.checked;
    var searchCustomerUrl = 'http://localhost:6160/api/Customers/Search?sortBy='+sortBy+'&descending='+descending+'&name='+keyword;
    xmlHttp.onreadystatechange = readyState;
    xmlHttp.open("GET",searchCustomerUrl,true);
    xmlHttp.send(null);
}

