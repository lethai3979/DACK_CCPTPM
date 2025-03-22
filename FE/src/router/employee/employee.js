const employee = [{
    path : "/employee",
    component: () => import("../../Views/ViewAdminEmployee.vue"),
    meta: { requiresRole: "Employee" },
    children: [
        {
            path : "SubmitDriver",
            name: "employee-driver",
            component: ()=> import("../../Views/Employee/Views/SendSubmit/Submit.vue"),
        },
        {
            path : "SubmitDriverDetail/:id",
            name: "employee-driver-detail",
            component: ()=> import("../../Views/Employee/Views/SendSubmit/DetailDiver.vue"),
        },
        {
            path: "report",
            name: "admin-employee-report",
            component: () => import("../../Views/Employee/Views/Reports/Index.vue"),
            meta: { requiresRole: ["Admin", "Employee"] }
        },
          {
            path: "report/detail/:id",
            name: "admin-report-detail", // Đổi tên để tránh xung đột
            component: () =>
              import("../../Views/Employee/Views/Reports/Details.vue"),
            meta: { requiresRole: ["Admin", "Employee"] }
        },
        // request
        {
            path: "refund",
            name: "admin-employee-refund",
            component: () => import("../../Views/Employee/Views/Bookings/IndexRefund.vue"),
            meta: { requiresRole: ["Admin", "Employee"] }
        },
        {
            path: "request",
            name: "admin-employee-request",
            component: () => import("../../Views/Employee/Views/Bookings/Index.vue"),
            meta: { requiresRole: ["Admin", "Employee"] }
        },
          {
            path: "request/detail/:id",
            name: "admin-request-detail", // Đổi tên để tránh xung đột
            component: () =>
              import("../../Views/Employee/Views/Bookings/Details.vue"),
            meta: { requiresRole: ["Admin", "Employee"] }
        },
        {
          path: "getallreport",
          name: "employee-getallreport", // Đổi tên để tránh xung đột
          component: () =>
            import("../../Views/Employee/PostReport/IndexReport.vue"),
          meta: { requiresRole: "Employee" }
        },
    ]
}];
export default employee;