const authen = [{
    path : "/authen",
    component: () => import("../../Views/Views.vue"),


    children: [
        {
            path : "dangnhap",
            name: "authen-dangnhap",
            component: ()=> import("../../Views/Authen/Login.vue"),
        }
    ]
}];
export default authen;