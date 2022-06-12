var returnxmlHttp = new XMLHttpRequest();

var customeridelement = document.getElementById('returncustomerid');
customeridelement.disabled=true;

var returnlistarray =[];
var fee;

function searchreturn()
{
    var select = document.getElementById('returndropdown');
    select.innerHTML='';
    var returnkeyword = document.getElementById('returnsearch').value;

    for(i=0; i<customerResponse.length ;i++)
    {
        var customercheck = customerResponse[i].customerName.toLowerCase();
        if(customercheck.includes(returnkeyword.toLowerCase()))
        {            
            var option = document.createElement('option');
            option.innerHTML = customerResponse[i].customerName;
            select.appendChild(option);
        }
     }
}

function returnshorttime(i)
{
    var returnDate = historyResponse.find(x=>x.rentalId == returnlistarray[i]).returnDate;
    var returndate = new Date(returnDate);
    var returndd = String(returndate.getDate()).padStart(2, '0');
    var mm = String(returndate.getMonth()+1).padStart(2, '0'); //January is 0!
    var yyyy = returndate.getFullYear();
    returndate = yyyy + '-' + mm + '-' + returndd;
    return(returndate);
}

function rentinglist()
{
    returnlistarray =[];
    fee=0;

    var customername = document.getElementById('returndropdown').value;
    document.getElementById('returncustomerlistname').innerHTML=customername+' renting list';
    document.getElementById('returnsearch').value=customername;
    var returncustomerid = customerResponse.find(x=>x.customerName == customername).customerID;
    customeridelement.value=returncustomerid;

    var rentingtable = document.getElementById('renting');
    rentingtable.innerHTML='<tr style="background-color: rgb(190, 190, 190);"><th>Book</th><th>Return Date</th><th></th></tr>';

    for(i=0;i<historyResponse.length;i++)
    {
        if(historyResponse[i].customerID == returncustomerid)
        {
            returnlistarray.push(historyResponse[i].rentalId);

            var tr = document.createElement('tr');
            if( i%2 != 0 )
                tr.setAttribute('style','background-color: rgb(240,240,240);');  
            rentingtable.appendChild(tr);

            var booknametd = document.createElement('td');
            booknametd.innerHTML = historyResponse[i].bookName;
            tr.appendChild(booknametd);

            var returndatetd = document.createElement('td');
            returndatetd.innerHTML = returnshorttime(i);
            tr.appendChild(returndatetd);

            var checkboxtd = document.createElement('td');
            checkboxtd.innerHTML= '<input type="checkbox" id="'+historyResponse[i].rentalId+'"onclick=checkbook()>';
            tr.appendChild(checkboxtd);
        }
    }
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function checkbook()
{
    fee = 0;
    for(i=0;i<returnlistarray.length;i++)
    {
        var position =returnlistarray[i];
        var check = document.getElementById(position);
        check = check.checked;
        late(check,i);
    }
}

function late(check,i)
{
    if(check)
    {
        var returnDate = new Date(returnshorttime(i));
        var today = new Date();
        var late = today-returnDate;
        late = Math.round(late/86400000); //Day
        if(late>0)
        {
            fee +=late*10;
        }
    }
    var latefee = document.getElementById('latefee');
    latefee.innerHTML = 'Late Fee:<br>'+fee+' Baht'; 
}

async function returnbook()
{
    for(i=0;i<returnlistarray.length;i++)
    {
        var position =returnlistarray[i];
        var check = document.getElementById(position);
        check = check.checked;
        if(check)
        {
            var bookUID = historyResponse.find(x=>x.rentalId == position).bookUID;
            var body= '{"customerID": "'+document.getElementById('returncustomerid').value+'",';
            body += '"bookUID": "'+bookUID+'"}';
            console.log(body);
            returnxmlHttp.open("POST",'http://localhost:6160/api/Customers/Return',true);
            returnxmlHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8")
            returnxmlHttp.send(body);
            await sleep(100);
        }
    }
    window.location.reload();
}