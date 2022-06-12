function edit(id)
{
    var editBookobj=bookResponse.find(x => x.bookUID === id);
    var editTable =document.getElementById('edittable');
    editTable.setAttribute("Style","border:1px solid black;display:true;")

    var bookUID=document.getElementById('bookUIDediter');
    bookUID.setAttribute('value',editBookobj.bookUID);
    bookUID.disabled=true;

    var malID = document.getElementById('editmalid');
    malID.setAttribute('value',editBookobj.malID);

    var editBookName=document.getElementById('editbookname');
    editBookName.setAttribute('value',editBookobj.bookName);

    var editInStock=document.getElementById('editinstock');
    editInStock.setAttribute('value',editBookobj.inStock);

    var editPrice=document.getElementById('editprice');
    editPrice.setAttribute('value',editBookobj.price);
}

function update()
{
    var updateid=document.getElementById('bookUIDediter').value;
    var putUrl = 'http://localhost:6160/api/Books?bookUID='+updateid;

    var malid = document.getElementById('editmalid').value;
    var bookName = document.getElementById('editbookname').value;
    var inStock=document.getElementById('editinstock').value;
    var price=document.getElementById('editprice').value;

    var updatebody = '{"bookName": "'+bookName+'","malID":"'+malid+'","inStock": '+inStock+',"price": '+price+'}';

    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("PUT",putUrl,true);
    xmlHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xmlHttp.send(updatebody);
    window.location.reload();
}

function deleted()
{
    var deleteid=document.getElementById('bookUIDediter').value;
    var deleteUrl = 'http://localhost:6160/api/Books?bookUID='+deleteid;

    var result = confirm("Want to delete?");
    if (result) 
    {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("DELETE",deleteUrl,true);
    xmlHttp.send(null);
    window.location.reload();
    }
}