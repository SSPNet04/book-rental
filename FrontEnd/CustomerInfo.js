function edit(id)
{
    var editCustomerobj=customerResponse.find(x => x.customerID === id);
    var editTable =document.getElementById('edittable');
    editTable.setAttribute("Style","border:1px solid black;display:true;")

    var customerID=document.getElementById('CustomerIDediter');
    customerID.setAttribute('value',editCustomerobj.customerID);
    customerID.disabled=true;

    var editCustomerName=document.getElementById('editcustomername');
    editCustomerName.setAttribute('value',editCustomerobj.customerName);

    var editPhone=document.getElementById('editphone');
    editPhone.setAttribute('value',editCustomerobj.phoneNumber);

}

function update()
{
    var updateid=document.getElementById('CustomerIDediter').value;
    var putUrl = 'http://localhost:6160/api/Customers?customerId='+updateid;

    var customerName = document.getElementById('editcustomername').value;
    var phone = document.getElementById('editphone').value;

    var updatebody = '{"customerName": "'+customerName+'","phoneNumber": "'+phone+'"}';

    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("PUT",putUrl,true);
    xmlHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xmlHttp.send(updatebody);
    //sleep(200);
    window.location.reload();
}

function deleted()
{
    var deleteid=document.getElementById('CustomerIDediter').value;
    var deleteUrl = 'http://localhost:6160/api/Customers?id='+deleteid;

    var result = confirm("Want to delete?");
    if (result) 
    {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("DELETE",deleteUrl,true);
    xmlHttp.send(null);
    window.location.reload();
    }
    if (xmlHttp.responseType==400)
    {
        alert('This Customer is renting');
    }
}

function info(id)
{
    var infoCustomerobj=customerResponse.find(x => x.customerID === id);
    var infoTable = document.getElementById('infotable');
    if(infoCustomerobj.bookRenting > 0)
    {
        infoTable.setAttribute("Style","border:1px solid black;display:true;");
        infoTable.innerHTML='<tr style="border:1px solid black;"><th colspan="2">Book Renting</th></tr><tr><th style="border:1px solid black; width:50%">Book Name</th><th>Return Date</th></tr>';

        var rentingUrl = 'http://localhost:6160/api/RentalHistory/Search?renting=true&id='+id;

        var rentingxmlHttp = new XMLHttpRequest();
        rentingxmlHttp.onreadystatechange= function()
        {   
        if(rentingxmlHttp.readyState==4)
        {
            if(rentingxmlHttp.status==200)
            {
                rentingResponse=JSON.parse(rentingxmlHttp.responseText);
                for(var ir=0;ir<rentingResponse.length;ir++)
                {
                    var rentingtr = document.createElement('tr');
                    document.getElementById('infotable').appendChild(rentingtr);
    
                    var bookNametd = document.createElement('td');
                    bookNametd.innerHTML=rentingResponse[ir].bookName;
                    rentingtr.appendChild(bookNametd);
    
                    var bookReturnDate = document.createElement('td');
                    bookReturnDate.innerHTML = rentingResponse[ir].returnDate;
                    rentingtr.appendChild(bookReturnDate);
                }
            }
        }
        };
        rentingxmlHttp.open("GET",rentingUrl,true);
        rentingxmlHttp.send(null);

    }
    else
        infoTable.setAttribute("Style","display:none;");
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
   