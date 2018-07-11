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
function searchWorkOrder(searchExp) {

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

<<<<<<< HEAD
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
        priceElem.value = price;
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

=======
>>>>>>> 4d41b0dee3981c841093d9a2d8af3bc87aba1f45



