const admin = [
  {
    path: "/admin",
    component: () => import("../../Views/ViewAdminEmployee.vue"), // Component cha
    meta: { requiresRole: "Admin" },
    children: [
      //Post
      {
        path: "postAdmin",
        name: "admin-postAdmin",
        component: () => import("../../Views/Admin/Views/Post/Index.vue"),
        meta: { requiresRole: "Admin" },
      },
      // Amenity
      {
        path: "amenity",
        name: "admin-amenity",
        component: () => import("../../Views/Admin/Views/Amenities/Index.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "amenity/create",
        name: "admin-amenity-create",
        component: () => import("../../Views/Admin/Views/Amenities/Create.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "amenity/detail/:id",
        name: "admin-amenity-detail",
        component: () =>
          import("../../Views/Admin/Views/Amenities/Details.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "amenity/edit/:id",
        name: "admin-amenity-edit",
        component: () => import("../../Views/Admin/Views/Amenities/Edit.vue"),
        meta: { requiresRole: "Admin" },
      },
      // Cartype
      {
        path: "cartype",
        name: "admin-cartype",
        component: () => import("../../Views/Admin/Views/CarTypes/Index.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "cartype/create",
        name: "admin-cartype-create", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/CarTypes/Create.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "cartype/detail/:id",
        name: "admin-cartype-detail", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/CarTypes/Details.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "cartype/edit/:id",
        name: "admin-cartype-edit", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/CarTypes/Edit.vue"),
        meta: { requiresRole: "Admin" },
      },
      // Company
      {
        path: "company",
        name: "admin-company",
        component: () => import("../../Views/Admin/Views/Companies/Index.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "company/create",
        name: "admin-company-create", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/Companies/Create.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "company/detail/:id",
        name: "admin-company-detail", // Đổi tên để tránh xung đột
        component: () =>
          import("../../Views/Admin/Views/Companies/Details.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "company/edit/:id",
        name: "admin-company-edit", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/Companies/Edit.vue"),
        meta: { requiresRole: "Admin" },
      },
      // SalePromotion
      {
        path: "Promotion",
        name: "admin-promotion",
        component: () =>
          import("../../Views/Admin/Views/AdminPromotions/Index.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "Promotion/create",
        name: "admin-promotion-create", // Đổi tên để tránh xung đột
        component: () =>
          import("../../Views/Admin/Views/AdminPromotions/Create.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "Promotion/detail/:id",
        name: "admin-promotion-detail", // Đổi tên để tránh xung đột
        component: () =>
          import("../../Views/Admin/Views/AdminPromotions/Details.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "Promotion/edit/:id",
        name: "admin-promotion-edit", // Đổi tên để tránh xung đột
        component: () =>
          import("../../Views/Admin/Views/AdminPromotions/Edit.vue"),
        meta: { requiresRole: "Admin" },
      },
      // ReportType
      {
        path: "reportType",
        name: "admin-reportType",
        component: () => import("../../Views/Admin/Views/ReportType/Index.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "reportType/create",
        name: "admin-reportType-create", // Đổi tên để tránh xung đột
        component: () =>
          import("../../Views/Admin/Views/ReportType/Create.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "reportType/detail/:id",
        name: "admin-reportType-detail", // Đổi tên để tránh xung đột
        component: () =>
          import("../../Views/Admin/Views/ReportType/Details.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "reportType/edit/:id",
        name: "admin-reportType-edit", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/ReportType/Edit.vue"),
        meta: { requiresRole: "Admin" },
      },
      {
        path: "admin-user",
        name: "admin-user", // Đổi tên để tránh xung đột
        component: () => import("../../Views/Admin/Views/Customers/Index.vue"),
        meta: { requiresRole: "Admin" },
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
    ],
  },
];

export default admin;

// const admin = [{
//     path : "/admin",
//     component: () => import("../../Views/Views.vue"),

//     children: [
//         // Amenity
//         {
//             path : "amenity",
//             name: "admin-amenity",
//             component: ()=> import("../../Views/Admin/Views/Amenities/Index.vue"),
//         },
//         {
//             path : "amenity/create",
//             name: "admin-create",
//             component: ()=> import("../../Views/Admin/Views/Amenities/Create.vue"),
//         },
//         {
//             path : "amenity/detail",
//             name: "admin-detail",
//             component: ()=> import("../../Views/Admin/Views/Amenities/Details.vue"),
//         },
//         {
//             path : "amenity/edit",
//             name: "admin-edit",
//             component: ()=> import("../../Views/Admin/Views/Amenities/Edit.vue"),
//         },
//         // Cartype
//         {
//             path : "cartype",
//             name: "admin-cartype",
//             component: ()=> import("../../Views/Admin/Views/CarTypes/Index.vue"),
//         },
//         {
//             path : "cartype/create",
//             name: "admin-create",
//             component: ()=> import("../../Views/Admin/Views/CarTypes/Create.vue"),
//         },
//         {
//             path : "cartype/detail",
//             name: "admin-detail",
//             component: ()=> import("../../Views/Admin/Views/CarTypes/Details.vue"),
//         },
//         {
//             path : "cartype/edit",
//             name: "admin-edit",
//             component: ()=> import("../../Views/Admin/Views/CarTypes/Edit.vue"),
//         },
//         // Company
//         {
//             path : "company",
//             name: "admin-company",
//             component: ()=> import("../../Views/Admin/Views/Companies/Index.vue"),
//         },
//         {
//             path : "company/create",
//             name: "admin-create",
//             component: ()=> import("../../Views/Admin/Views/Companies/Create.vue"),
//         },
//         {
//             path : "company/detail",
//             name: "admin-detail",
//             component: ()=> import("../../Views/Admin/Views/Companies/Details.vue"),
//         },
//         {
//             path : "company/edit",
//             name: "admin-edit",
//             component: ()=> import("../../Views/Admin/Views/Companies/Edit.vue"),
//         },
//         // SalePromotion
//         {
//             path : "salePromotion",
//             name: "admin-salepromotion",
//             component: ()=> import("../../Views/Admin/Views/SalePromotions/Index.vue"),
//         },
//         {
//             path : "salePromotion/create",
//             name: "admin-create",
//             component: ()=> import("../../Views/Admin/Views/SalePromotions/Create.vue"),
//         },
//         {
//             path : "salePromotion/detail",
//             name: "admin-detail",
//             component: ()=> import("../../Views/Admin/Views/SalePromotions/Details.vue"),
//         },
//         {
//             path : "salePromotion/edit",
//             name: "admin-edit",
//             component: ()=> import("../../Views/Admin/Views/SalePromotions/Edit.vue"),
//         },

//     ]
// }];
// export default admin;
