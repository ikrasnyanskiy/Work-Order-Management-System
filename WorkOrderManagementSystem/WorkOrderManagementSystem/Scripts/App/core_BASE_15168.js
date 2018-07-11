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