// **search for customers: populates DOM with id="custList"
function searchCust(searchExp) {

  searchExp = searchExp.trim();
  // Get a reference to the DOM element
  var element = document.querySelector('#custList');// NOTE the '#' preceding the name of the div we're updating

  // create an xhr object
  var xhr = new XMLHttpRequest();

  // configure its handler
  xhr.onreadystatechange = function () {

    if (xhr.readyState === 4) {
      // request-response cycle has been completed, so continue

      if (xhr.status === 200) {
        // request was successfully completed, and data was received, so continue

        // update the user interface
        element.innerHTML = xhr.responseText;



      } else {
        element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    } else {
      //e.innerHTML = "<p>Loading...</p>";
    }
    // show the content
    element.style.display = 'block';
  }

  // configure the xhr object to fetch content
  xhr.open('GET', '/WorkOrders/customerSearch/?searchExp=' + searchExp, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
  // fetch the content
  xhr.send(null);
}

// **search for workorder: populates DOM with id="workordersearch"
function searchActiveWorkOrder(searchExp) {

    searchExp = searchExp.trim();
    // Get a reference to the DOM element
    var element = document.querySelector('#WorkOrderTable');// NOTE the '#' preceding the name of the div we're updating

    // create an xhr object
    var xhr = new XMLHttpRequest();

    // configure its handler
    xhr.onreadystatechange = function () {

        if (xhr.readyState === 4) {
            // request-response cycle has been completed, so continue

            if (xhr.status === 200) {
                // request was successfully completed, and data was received, so continue

                // update the user interface
                element.innerHTML = xhr.responseText;



            } else {
                element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
            }
        } else {
            //e.innerHTML = "<p>Loading...</p>";
        }
        // show the content
        element.style.display = 'block';
    }

    // configure the xhr object to fetch content
    xhr.open('GET', '/WorkOrders/WorkOrderSearch/?searchExp=' + searchExp, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
    // fetch the content
    xhr.send(null);
}
function searchCompletedWorkOrder(searchExp) {

  searchExp = searchExp.trim();
  // Get a reference to the DOM element
  var element = document.querySelector('#WorkOrderTable');// NOTE the '#' preceding the name of the div we're updating

  // create an xhr object
  var xhr = new XMLHttpRequest();

  // configure its handler
  xhr.onreadystatechange = function () {

    if (xhr.readyState === 4) {
      // request-response cycle has been completed, so continue

      if (xhr.status === 200) {
        // request was successfully completed, and data was received, so continue

        // update the user interface
        element.innerHTML = xhr.responseText;



      } else {
        element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    } else {
      //e.innerHTML = "<p>Loading...</p>";
    }
    // show the content
    element.style.display = 'block';
  }

  // configure the xhr object to fetch content
  xhr.open('GET', '/WorkOrders/WorkOrderCompletedSearch/?searchExp=' + searchExp, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
  // fetch the content
  xhr.send(null);
}

function searchAllWorkOrder(searchExp) {

  searchExp = searchExp.trim();
  // Get a reference to the DOM element
  var element = document.querySelector('#WorkOrderTable');// NOTE the '#' preceding the name of the div we're updating

  // create an xhr object
  var xhr = new XMLHttpRequest();

  // configure its handler
  xhr.onreadystatechange = function () {

    if (xhr.readyState === 4) {
      // request-response cycle has been completed, so continue

      if (xhr.status === 200) {
        // request was successfully completed, and data was received, so continue

        // update the user interface
        element.innerHTML = xhr.responseText;



      } else {
        element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    } else {
      //e.innerHTML = "<p>Loading...</p>";
    }
    // show the content
    element.style.display = 'block';
  }

  // configure the xhr object to fetch content
  xhr.open('GET', '/WorkOrders/WorkOrderAllSearch/?searchExp=' + searchExp, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
  // fetch the content
  xhr.send(null);
}

//Get list of bicycles for customer with input arg's id
//will populate lists with id="bikeList"
function getBikeList(custId) {

  
  // Get a reference to the DOM element
  var element = document.querySelector('#bikeList');// NOTE the '#' preceding the name of the div we're updating

  // create an xhr object
  var xhr = new XMLHttpRequest();

  // configure its handler
  xhr.onreadystatechange = function () {

    if (xhr.readyState === 4) {
      // request-response cycle has been completed, so continue

      if (xhr.status === 200) {
        // request was successfully completed, and data was received, so continue

        // update the user interface
        element.innerHTML = xhr.responseText;



      } else {
        element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    } else {
      //e.innerHTML = "<p>Loading...</p>";
    }
    // show the content
    element.style.display = 'block';
  }

  // configure the xhr object to fetch content
  xhr.open('GET', '/WorkOrders/bicycleSearch/?custId=' + custId.value, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
  // fetch the content
  xhr.send(null);
}

// **search for mechanics: populates DOM with id="mechList"
function searchMech(searchExp) {

  searchExp = searchExp.trim();
  // Get a reference to the DOM element
  var element = document.querySelector('#mechList');// NOTE the '#' preceding the name of the div we're updating

  // create an xhr object
  var xhr = new XMLHttpRequest();

  // configure its handler
  xhr.onreadystatechange = function () {

    if (xhr.readyState === 4) {
      // request-response cycle has been completed, so continue

      if (xhr.status === 200) {
        // request was successfully completed, and data was received, so continue

        // update the user interface
        element.innerHTML = xhr.responseText;



      } else {
        element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    } else {
      //e.innerHTML = "<p>Loading...</p>";
    }
    // show the content
    element.style.display = 'block';
  }

  // configure the xhr object to fetch content
  xhr.open('GET', '/WorkOrders/mechanicSearch/?searchExp=' + searchExp, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
  // fetch the content
  xhr.send(null);
}

function searchCustomer(searchExp) {

    searchExp = searchExp.trim();
    // Get a reference to the DOM element
    var element = document.querySelector('#CustomerTable');// NOTE the '#' preceding the name of the div we're updating

    // create an xhr object
    var xhr = new XMLHttpRequest();

    // configure its handler
    xhr.onreadystatechange = function () {

        if (xhr.readyState === 4) {
            // request-response cycle has been completed, so continue

            if (xhr.status === 200) {
                // request was successfully completed, and data was received, so continue

                // update the user interface
                element.innerHTML = xhr.responseText;



            } else {
                element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
            }
        } else {
            //e.innerHTML = "<p>Loading...</p>";
        }
        // show the content
        element.style.display = 'block';
    }

    // configure the xhr object to fetch content
    xhr.open('GET', '/Customers/CustomerSearch/?searchExp=' + searchExp, true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
    // fetch the content
    xhr.send(null);
}
//function getCustCreateForModal() {

//    // searchExp = searchExp.trim();
//    // Get a reference to the DOM element

//    //var element = document.querySelector('#createCustModal');
//    console.log('good');

//    // create an xhr object
//    var xhr = new XMLHttpRequest();

//    // configure its handler
//    xhr.onreadystatechange = function () {

//        if (xhr.readyState === 4) {
//            // request-response cycle has been completed, so continue

//            if (xhr.status === 200) {
//                // request was successfully completed, and data was received, so continue

//                // update the user interface
//                element.innerHTML = xhr.responseText;



//            } else {
//                element.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
//            }
//        } else {
//            //e.innerHTML = "<p>Loading...</p>";
//        }
//        // show the content
//        element.style.display = 'block';
//    }

//    // configure the xhr object to fetch content
//    xhr.open('GET', '/WorkOrders/createCustWO', true);//TODO: ADDED GET style variable syntax to request URL to allow controller to recognize variable
//    // fetch the content
//    xhr.send(null);
//}


//Change Work Order Status
function changeStatus(workOrderId) {

    var elem = document.querySelector('#WorkOrderTable'); 
    //create xhr object
    var xhr = new XMLHttpRequest();

    //configure the handler of it
    xhr.onreadystatechange = function () {

        //if request-response cycle has been completed
        if (xhr.readyState === 4) {
            //if it returns something viable
            if (xhr.status === 200) {
                elem.innerHTML = xhr.responseText;
            } else {
                elem.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
            }
        }
        //show the content
        elem.style.display = 'block';
    }
    //configure the xhr object to fetch the content
    xhr.open('GET', '/WorkOrders/changeStatus/?Id=' + workOrderId, true);

    xhr.send(null);
}
//Change Work Order Status -- above method calls manager method that returns a get active vs get completed view based on initial order status
//this function calls a different manager method that returns a get all collection
function changeStatusFromAllView(workOrderId) {

    var elem = document.querySelector('#WorkOrderTable'); 
    //create xhr object
    var xhr = new XMLHttpRequest();

    //configure the handler of it
    xhr.onreadystatechange = function () {

        //if request-response cycle has been completed
        if (xhr.readyState === 4) {
            //if it returns something viable
            if (xhr.status === 200) {
                elem.innerHTML = xhr.responseText;
            } else {
                elem.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
            }
        }
        //show the content
        elem.style.display = 'block';
    }
    //configure the xhr object to fetch the content
    xhr.open('GET', '/WorkOrders/changeStatusFromAllView/?Id=' + workOrderId, true);

    xhr.send(null);
}
//Change Work Order Status from mechanic view:
//fills labelled div with partial voew containing 2 tables:
//active and inactive workorders based on mechanic's work logs  
function changeStatusFromMechView(mechId, workOrderId) {//currently unused

  var elem = document.querySelector('#WorkOrderTables');
  //create xhr object
  var xhr = new XMLHttpRequest();

  //configure the handler of it
  xhr.onreadystatechange = function () {

    //if request-response cycle has been completed
    if (xhr.readyState === 4) {
      //if it returns something viable
      if (xhr.status === 200) {
        elem.innerHTML = xhr.responseText;
      } else {
        elem.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    }
    //show the content
    elem.style.display = 'block';
  }
  //configure the xhr object to fetch the content
  xhr.open('GET', '/WorkOrders/changeStatusFromMechView/?mechId=' + parseInt(mechId) + "&workOrderId="+ parseInt(workOrderId), true);

  xhr.send(null);
}

function priceForOrderLine(servListElem, quantElem, priceElem) {//pulls id number from serviceitemselectlist on form and uses it to set price of same id# for same row
  //var elemId = serviceList.id.split('m')[1];//split off number after m in "Serice"
  //elemId = parseInt(elemId, 10);
 // var findMe = '#'+ priceId.toString()
  
  //create xhr object
  var xhr = new XMLHttpRequest();

  //configure the handler of it
  xhr.onreadystatechange = function () {

    //if request-response cycle has been completed
    if (xhr.readyState === 4) {
      //if it returns something viable
      if (xhr.status === 200) {
        var price = xhr.responseText;
        price = parseInt(price);
        price = price * quantElem.value;
        if (isNaN(price))
        {
            priceElem.value = "invalid quantity";
        }
        else
        {
          priceElem.value = price.toFixed(2);
          calcOrderTotal();
        }
      } else {
        priceElem.value = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
      }
    }
    //show the content
    //elem.style.display = 'block';
  }
  //configure the xhr object to fetch the content
  xhr.open('GET', '/ServiceItems/getPriceById/?id=' + servListElem.value, true);

  xhr.send(null);
}


//for search boxes: will listen for enter press in textbox and call click event on associated button.
//To work, you must give the searchbox an id that ends with "Box" and the associated button an id ending with "Btn"
function submitSearch(event) {
  var textElement = event.target;//use instead of SrcElement, which is depreciated
  var btnName = textElement.id.replace("Box", "Btn");
  var targetBtn = document.getElementById(btnName);
  if (event.keyCode === 13 || event.which ===13) {
    targetBtn.click();
    event.preventDefault();//prevent default event handling for pressing enter (which is to submit the form)
    event.stopPropagation();
  }
}

function clearSelected() {
    var list = document.getElementById("ActiveMechanicId").options;
    for (var i = 0; i < list.length; i++) {
        list[i].selected = false;
    }
}

//Invoice Paid Status Changer
function changePaidStatus(id) {

//might have to check this is the corrct document.query
    var elem = document.querySelector('#InvoiceMoreDetailed');
    //create xhr object
    var xhr = new XMLHttpRequest();

    //configure the handler of it
    xhr.onreadystatechange = function () {

        //if request-response cycle has been completed
        if (xhr.readyState === 4) {
            //if it returns something viable
            if (xhr.status === 200) {
                elem.innerHTML = xhr.responseText;
            } else {
                elem.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
            }
        }
        //show the content
        elem.style.display = 'block';
    }
    //configure the xhr object to fetch the content
    xhr.open('GET', '/Invoice/changePaidStatus/?id=' + id, true);

    xhr.send(null);
}

