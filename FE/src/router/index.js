import { createRouter, createWebHistory } from 'vue-router';
import admin from './admin/admin.js';
import authen from './authen/authen.js';
import HomeView from '../Views/Home/Index.vue';
import HomeUser from './home.js';
import customer from './customer/customer.js';
import employee from './employee/employee.js';
import driver from './driver/driver.js';

// Định nghĩa các route chính
const routes = [
    ...admin,
    ...employee,
    ...authen,
    ...customer,
    ...driver,
    ...HomeUser,
    {
        path: '/unauthorized',
        name: 'unauthorized',
        component: () => import("../Views/Unauthoried.vue"), // Trang Unauthorized
    },
    {
        path: '/',
        redirect: '/Home',
    },
    {
        path: '/Home',
        name: 'HomeView',
        component: HomeView,
    },
];

// Tạo router
const router = createRouter({
    history: createWebHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        // Nếu đã lưu vị trí cuộn (khi dùng nút quay lại/tiến tới)
        if (savedPosition) {
            return savedPosition;
        }
        // Mặc định cuộn về đầu trang
        return { top: 0 };
    },
});

// Hàm lấy role của người dùng từ sessionStorage
// Hàm lấy danh sách role của user từ sessionStorage
function getUserRoles() {
    const user = JSON.parse(sessionStorage.getItem('User'));
    return user?.roles || ['guest']; // Nếu không có, trả về danh sách với 'guest' mặc định
}

// Hàm kiểm tra quyền truy cập
router.beforeEach((to, from, next) => {
    const requiredRoles = to.meta.requiresRole; // Có thể là mảng hoặc chuỗi
    const userRoles = getUserRoles(); // Lấy danh sách role của người dùng từ sessionStorage

    console.log('Required Roles:', requiredRoles);
    console.log('User Roles:', userRoles);

    // Nếu `requiredRoles` là mảng, kiểm tra ít nhất một role của user có nằm trong requiredRoles
    if (requiredRoles && Array.isArray(requiredRoles)) {
        const hasAccess = userRoles.some(role => requiredRoles.includes(role));
        if (!hasAccess) {
            return next({ name: 'unauthorized' }); // Chuyển hướng đến trang không có quyền truy cập
        }
    } else if (requiredRoles && !userRoles.includes(requiredRoles)) {
        // Nếu `requiredRoles` là chuỗi, kiểm tra xem userRoles có chứa requiredRole hay không
        return next({ name: 'unauthorized' }); // Chuyển hướng đến trang không có quyền truy cập
    }

    next(); // Cho phép truy cập nếu mọi điều kiện đều đúng
});

export default router;





// import {createRouter,createWebHistory} from 'vue-router';
// import admin from './admin/admin.js';
// import authen from './authen/authen.js';
// import HomeView from '../Views/ViewsUser.vue';
// import HomeUser from './home.js'
// import customer from './customer/customer.js';
// import employee from './employee/employee.js'
// const routes = [...admin,...employee,...authen,...customer,...HomeUser,{
//     path: '/unauthorized',
//     name: 'unauthorized',
//     component: () => import("../Views/Unauthoried.vue") // Trang Unauthorized
//     },
//     {
//         path: '/',
//         redirect: '/Home' 
//     },
//     {
//         path: '/Home',
//         name: 'HomeView',
//         component: HomeView
//     },
// ];

// const router = createRouter({
//     history: createWebHistory(),
//     routes
// })

// function getUserRole() {
//     const user = JSON.parse(sessionStorage.getItem('User'));
//     return user?.role || 'guest';  
// }


// // router.beforeEach((to, from, next) => {
// //     const requiredRole = to.meta.requiresRole; 
// //     const userRole = getUserRole();  
// //      console.log('Required Role:', requiredRole);
// //      console.log('User Role:', userRole);

// //     if (requiredRole && requiredRole !== userRole) {
// //         return next({ name: 'unauthorized' });
// //     }
// //     next();
// // });
// router.beforeEach((to, from, next) => {
//     const requiredRole = to.meta.requiresRole; // ['Admin', 'Employee']
//     const userRole = getUserRole(); // 'Employee'
    
//     // console.log('Required Role:', requiredRole);
//     // console.log('User Role:', userRole);

//     // Kiểm tra nếu `requiredRole` là mảng và `userRole` nằm trong mảng đó
//     if (requiredRole && Array.isArray(requiredRole)) {
//         if (!requiredRole.includes(userRole)) {
//             return next({ name: 'unauthorized' }); // Chuyển hướng đến route không có quyền truy cập
//         }
//     } else if (requiredRole && requiredRole !== userRole) {
//         // Trường hợp `requiredRole` không phải là mảng
//         return next({ name: 'unauthorized' });
//     }

//     next(); // Cho phép truy cập nếu tất cả các điều kiện đều đúng
// });

  

// export default router;