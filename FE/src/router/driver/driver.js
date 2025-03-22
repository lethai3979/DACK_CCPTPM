const user = [
    {
      path: "/driver",
      component: () => import("../../Views/ViewsUser.vue"),
      children: [
        {
          path: "list",
          name: "driver-list",
          component: () => import("../../Views/Driver/ListBookingDriver.vue"),
          meta: { requiresRole: 'Driver' }
        },
        {
          path: "details:id",
          name: "driver-detail",
          component: () => import("../../Views/Driver/DetailsBookingDriver.vue"),
          meta: { requiresRole: 'Driver' }
        },
        
      ],
    },
  ];
  export default user;
  