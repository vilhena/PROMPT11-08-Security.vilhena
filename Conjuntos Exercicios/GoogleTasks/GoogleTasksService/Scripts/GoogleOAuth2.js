/// <reference path="http://localhost:5555/Scripts/jquery-1.5.1.js" />

$(function () {

    if (window.location.hash) {
        // Fragment exists

        var params = {}, queryString = location.hash.substring(1),
        regex = /([^&=]+)=([^&]*)/g, m;
        while (m = regex.exec(queryString)) {
            params[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
        }

        $.getJSON('http://' + window.location.host + '/Google/Catchtoken?' + queryString, null,
        function (data) {;
            $('#googleresults').css('border','3px solid red');
            $('#googleresults').text(data.items[0].title);
        });


        //        // And send the token over to the server
        //        var req = new XMLHttpRequest();
        //        // consider using POST so query isn't logged
        //        req.open('GET', 'http://' + window.location.host + '/Google/Catchtoken?' + queryString, true);

        //        req.onreadystatechange = function (e) {
        //            if (req.readyState == 4) {
        //                alert(req.readyState);
        //                if (req.status == 200) {
        //                    alert(e.toString());
        //                    window.location = params['state']
        //                }
        //                else if (req.status == 400) {
        //                    alert('There was an error processing the token.')
        //                }
        //                else {
        //                    alert('something else other than 200 was returned')
        //                }
        //            }
        //        };
        //        req.send(null);

    } else {
        // Fragment doesn't exist
        alert("no fragment");
    }

});