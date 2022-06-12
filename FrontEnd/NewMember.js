function newmember()
{
    var postHttp = new XMLHttpRequest();
    var customerId = document.getElementById('newcustomerid').value;
    var customerName = document.getElementById('newcustomername').value;
    var phone = document.getElementById('newphone').value;
    var newCustomer ='{"customerID": "'+customerId+'","customerName": "'+customerName+'","phoneNumber": "'+phone+'"}';
    var postCustomerUrl = 'http://localhost:6160/api/Customers';
    
    if (customerId=='')
    {
        alert('ID Field Required');
        return;
    }

    postHttp.onreadystatechange = function()
    {
        if (postHttp.readyState == 4)
        {
            if(postHttp.response == 'This ID already register')
                alert('This ID already redistered');
            else
                window.location.reload();
        }

    }
    postHttp.open("POST",postCustomerUrl,true);
    postHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    postHttp.send(newCustomer);

    
}
