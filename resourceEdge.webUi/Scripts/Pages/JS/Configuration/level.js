﻿
(function () {
    var count = 0;
    $('#add').on('click', function () {
        var current = count++;
        console.log(current);
        if (current < 2) {
            $('#levelForm').append(`
        <div class ="col-md-6">
            <label for="LevelName" class ="control-label">Name</label>
            <input name="LevelName[${count}]" id="City" class ="form-control" placeholder="Name for level" required />
        </div>

        <div class ="col-md-6">
            <label for="EligibleYears" class ="control-label">Eligible Year(s) </label>
            <input name="EligibleYears[${count}]" id="eNo" class ="form-control" placeholder="Eligible years for level (Only Numbers)"  data-parsley-min="0" required="" />
        </div>

        <div class ="col-md-6">
            <label for="levelNo" class ="control-label">Level Number</label>
            <input name="levelNo[${count}] id="lNo" class ="form-control" placeholder="level number in ranking (Only Numbers)" data-parsley-min="0" required="" />
        </div>
          <div class ="col-md-12">
            <hr />
        </div>
                `);
        }
        else {
            $('#overload').append(`<div class="alert alert-danger alert-dismissable">
                <button type="button" class ="close" data-dismiss="alert" aria-hidden="true">&times; </button>
    <span>Sorry you can't add more than Three(3) Level at a time. kindly submit and then add again</span>
</div>`);
        }
    })

    
    $('#eNo').on('keypress', function (event) {
        return $.ValidateNumber(event, this);
    })

    $('#lNo').on('keypress', function (event) {
        return $.ValidateNumber(event, this);
    })
})();