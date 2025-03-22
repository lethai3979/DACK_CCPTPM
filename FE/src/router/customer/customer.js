const user = [
  {
    path: "/customer",
    component: () => import("../../Views/ViewsUser.vue"),
    children: [
      
      {
        path: "chart",
        name: "chart-invoice",
        component: () => import("../../Views/Customer/Views/Chart/Chart.vue"),
        meta: { requiresRole: 'User' }
      },
      {
        path: "dieukhoan",
        name: "dieukhoan",
        component: () => import("../../Views/Home/DieuKhoan.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "chinhsach",
        name: "chinhsach",
        component: () => import("../../Views/Home/Chinhsach.vue"),
        // meta: { requiresRole: 'User' }
      },
      //Post
      {
        path: "post-search",
        name: "post-search",
        component: () => import("../../Views/Home/ViewDemo.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "infor:userId?",
        name: "user-infor",
        component: () => import("../../Views/Home/FindUser.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "chinhsach",
        name: "chinhsach",
        component: () => import("../../Views/Home/Chinhsach.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "dieukhoan",
        name: "dieukhoan",
        component: () => import("../../Views/Home/DieuKhoan.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "gioithieu",
        name: "gioithieu",
        component: () => import("../../Views/Home/GioiThieu.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "chatbox",
        name: "user-chatbox",
        component: () => import("../../Views/ChatBox.vue"),
        meta: { requiresRole: 'User' }
      },
      {
        path: "post",
        name: "user-post",
        component: () => import("../../Views/Customer/Views/Posts/Index.vue"),
        // meta: { requiresRole: 'User' }
      },
      {
        path: "post/create",
        name: "user-post-create",
        component: () => import("../../Views/Customer/Views/Posts/Create.vue"),
        meta: { requiresRole: 'User' }
      },
      {
        path: "post/detail:id?",
        name: "user-post-detail",
        component: () => import("../../Views/Customer/Views/Posts/Details.vue"),
        
      },
      {
        path: "post/edit:id?",
        name: "user-post-edit",
        component: () => import("../../Views/Customer/Views/Posts/Edit.vue"),
        meta: { requiresRole: 'User' }
      },
      //Booking
      {
        path: "booking",
        name: "user-booking",
        component: () =>
          import("../../Views/Customer/Views/Bookings/Index.vue"),
        meta: { requiresRole: "User" },
      },
      {
        path: "booking/create",
        name: "user-booking-create",
        component: () =>
          import("../../Views/Customer/Views/Bookings/Create.vue"),
        meta: { requiresRole: "User" },
      },
      {
        path: "booking/detail:id?",
        name: "user-booking-detail",
        component: () =>
          import("../../Views/Customer/Views/Bookings/Details.vue"),
        meta: { requiresRole: "User" },
      },
      //Favorites
      {
        path: "favorites",
        name: "user-favorites",
        component: () =>
          import("../../Views/Customer/Views/Favorites/FavoriteList.vue"),
        meta: { requiresRole: "User" },
      },
      //Invoices
      {
        path: "invoices",
        name: "user-invoices",
        component: () =>
          import("../../Views/Customer/Views/Invoices/Index.vue"),
        meta: { requiresRole: "User" },
      },
      // {
      //     path : "invoices/create",
      //     name: "user-invoices-create",
      //     component: ()=> import("../../Views/Customer/Views/Invoices/Create.vue"),
      // },
      {
        path: "invoices/detail:id?",
        name: "user-invoices-detail",
        component: () =>
          import("../../Views/Customer/Views/Invoices/Details.vue"),
        meta: { requiresRole: "User" },
      },
      // {
      //     path : "invoices/edit:id",
      //     name: "user-invoices-edit",
      //     component: ()=> import("../../Views/Customer/Views/Invoices/Edit.vue"),
      // },
      //Notification
      {
        path: "notification",
        name: "user-notification",
        component: () =>
          import("../../Views/Customer/Views/Notification/Index.vue"),
        meta: { requiresRole: "User" },
      },
      {
        path: "notification/create",
        name: "user-notification-create",
        component: () =>
          import("../../Views/Customer/Views/Notification/Create.vue"),
        meta: { requiresRole: "User" },
      },
      {
        path: "notification/detail:id?",
        name: "user-notification-detail",
        component: () =>
          import("../../Views/Customer/Views/Notification/Details.vue"),
        meta: { requiresRole: "User" },
      },
      {
        path: "notification/edit:id?",
        name: "user-notification-edit",
        component: () =>
          import("../../Views/Customer/Views/Notification/Edit.vue"),
        meta: { requiresRole: "User" },
      },
      //Rating
      // {
      //     path : "rating",
      //     name: "user-rating",
      //     component: ()=> import("../../Views/Customer/Views/Rating/Index.vue"),
      // },
      {
        path: "rating/create",
        name: "user-rating-create",
        component: () =>
          import("../../Views/Customer/Views/Rating/AddComment.vue"),
        meta: { requiresRole: "User" },
      },
      // {
      //     path : "rating/detail:id",
      //     name: "user-rating-detail",
      //     component: ()=> import("../../Views/Customer/Views/Rating/Details.vue"),
      // },
      // {
      //     path : "rating/edit:id",
      //     name: "user-rating-edit",
      //     component: ()=> import("../../Views/Customer/Views/Rating/Edit.vue"),
      // },
    ],
  },
];
export default user;
