const infor = [
  {
    path: "/Information",
    component: () => import("../Views/Views.vue"),
    meta: { requiresRole: "User" },
    children: [
      //Information
      {
        path: "User",
        name: "user-profile",
        component: () => import("../Views/Home/User/Information.vue"),
        meta: { requiresRole: "User" },
        children: [
          {
            path: "MyCar",
            name: "user-profile-mycar",
            component: () => import("../Views/Home/User/MyCar.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "Driver",
            name: "user-profile-driver",
            component: () => import("../Views/Home/User/BookingDriver.vue"),
            meta: { requiresRole: "Driver" },
          },
          {
            path: "User",
            name: "user-profile-user",
            component: () => import("../Views/Home/User/BookingUser.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "VoucherList",
            name: "user-profile-voucherList",
            component: () => import("../Views/Home/User/VoucherList.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "Inforuser",
            name: "user-profile-inforuser",
            component: () => import("../Views/Home/User/InforUser.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "HistoryBooking",
            name: "user-profile-historyBooking",
            component: () => import("../Views/Home/User/HistoryBooking.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "HistoryInvoice",
            name: "user-profile-historyInvoice",
            component: () => import("../Views/Home/User/HistoryInvoice.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "FavoriteList",
            name: "user-profile-favoriteList",
            component: () => import("../Views/Home/User/FavoriteList.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "ChangEmail",
            name: "user-profile-changEmail",
            component: () => import("../Views/Home/User/ChangeEmail.vue"),
            meta: { requiresRole: "User" },
          },
          {
            path: "ChangPassword",
            name: "user-profile-changPassword",
            component: () => import("../Views/Home/User/ChangePassword.vue"),
            meta: { requiresRole: "User" },
          },
        ],
      },
    ],
  },
];
export default infor;
