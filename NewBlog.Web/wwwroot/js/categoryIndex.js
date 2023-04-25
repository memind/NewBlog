$(document).ready(function () {
    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "No data in table",
            "sInfo": "Showing records _START_ - _END_ of _TOTAL_ records",
            "sInfoEmpty": "No Record",
            "sInfoFiltered": "(Found in _MAX_ record.)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Show _MENU_ records in page",
            "sLoadingRecords": "Loading...",
            "sProcessing": "Fetching...",
            "sSearch": "Search:",
            "sZeroRecords": "No match",
            "oPaginate": {
                "sFirst": "First",
                "sLast": "Last",
                "sNext": "Next",
                "sPrevious": "Previous"
            },
            "oAria": {
                "sSortAscending": ": Sort Ascending",
                "sSortDescending": ": Sort Descending"
            },
            "select": {
                "rows": {
                    "_": "%d records selected",
                    "0": "",
                    "1": "1 record selected"
                }
            }
        }
    });
});