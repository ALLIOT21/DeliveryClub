function toggleActivity(id) {
    jQuery.ajax({
        url: `/api/dispatcher/${id}/active`,
        type: "POST",
        dataType: "json",
        success: function () {
            var checkbox = document.getElementById(`toggle-activity-${id}`);
            checkbox.setAttribute("checked", !checkbox.getAttribute("checked"));
        }
    })
}