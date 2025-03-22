<template>
    <div class="infor" v-if="user != null">
        <div v-if="logout == true" class="logout" style="z-index: 9999999999999999999; ">
            <h3>Bạn có chắc chắn muốn đăng xuất</h3>
            <div style="display: flex; gap: 10px;">
                <button @click="LogOut()" class="btn nutshort">Đăng xuất</button>
                <button @click="logout = false" class="btn1 nutshort">Không</button>
            </div>

        </div>
        <div class="sidebar_left">

            <div class="name_img">GoWheels Xin chào</div>

            <hr width="100%" style="margin-block: 10px;color: #ccc;">

            <button style="margin-top: 0;" :class="{ click: isActiveRoute('user-profile-inforuser') }"
                @click="goToRoute('user-profile-inforuser')">
                Thông tin cá nhân
            </button>
            <button :class="{ click: isActiveRoute('user-profile-mycar') }" @click="goToRoute('user-profile-mycar')">
                Xe cho thuê của bạn
            </button>
            <button v-if="user.roles.includes('Driver')" :class="{ click: isActiveRoute('user-profile-driver') }"
                @click="goToRoute('user-profile-driver')">
                Đơn đặt tài xế
            </button>
            <button :class="{ click: isActiveRoute('user-profile-user') }"
                @click="goToRoute('user-profile-user')">
                Lịch sử cho thuê xe
            </button>
            <button :class="{ click: isActiveRoute('user-profile-historyInvoice') }"
                @click="goToRoute('user-profile-historyInvoice')">
                Lịch sử giao dịch
            </button>
            <button :class="{ click: isActiveRoute('user-profile-historyBooking') }"
                @click="goToRoute('user-profile-historyBooking')">
                Lịch sử đặt xe
            </button>
            <button :class="{ click: isActiveRoute('user-profile-favoriteList') }"
                @click="goToRoute('user-profile-favoriteList')">
                Danh sách yêu thích
            </button>
            <button :class="{ click: isActiveRoute('user-profile-voucherList') }"
                @click="goToRoute('user-profile-voucherList')">
                Danh sách khuyến mãi
            </button>
            <!-- <button :class="{ click: isActiveRoute('user-profile-changEmail') }"
                @click="goToRoute('user-profile-changEmail')">
                Thay đổi Email
            </button>
            <button :class="{ click: isActiveRoute('user-profile-changPassword') }"
                @click="goToRoute('user-profile-changPassword')">
                Thay đổi password
            </button> -->
            <button :class="{ click: logout == true }" @click="logout = true">Đăng xuất</button>
        </div>
        <div class="sidebar_right">
            <router-view></router-view>
        </div>
    </div>
</template>

<script>
import UserVM from '../../../Model/UserVM';
import AuthenticationService from '../../../Service/api/AuthenticationService';
import TokenService from '../../../Service/authen/TokenService';

export default {
    data() {
        return {
            page: 0,
            user: null,
            logout: false
        }
    },
    methods: {
        async LogOut() {
            const response = await AuthenticationService.Logout();
            const haha = TokenService.removeToken();
            sessionStorage.setItem('LogOut', true);
            const logout = sessionStorage.getItem('LogOut');
            console.log(logout);
            // window.location.reload();
            this.logout = false;
            this.$router.push({ path: '/Home' });
        },
        isActiveRoute(routeName) {
            return this.$route.name === routeName;
        },
        // Hàm chuyển hướng đến route cụ thể
        goToRoute(routeName) {
            this.$router.push({ name: routeName });
        }
    },
    created() {
        const userString = sessionStorage.getItem('User');
        this.user = JSON.parse(userString);
    },
}
</script>

<style>
.logout {
    width: 40.3%;
    position: absolute;
    background-color: aliceblue;
    padding: 40px 50px;
    border-radius: 15px;
    margin-top: 15%;
    margin-inline: 30%;
    align-items: center;
    align-content: center;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);    
}

.name_img {
    padding: 10px 20px;
    margin-top: 5px;
    font-size: 18px;
    color: #0b0b0b;
}

.infor {
    margin-top: 1px;
    width: 100%;
    display: flex;
    height: 643px;
}

.sidebar_left {
    width: 23%;
}

.sidebar_left button {
    width: 100%;
    text-align: left;
    padding: 15px 10px;
    background-color: var(--bg-colorBackground);
    color: #909090;
    border: none;
}

.sidebar_left button.click {
    background-color: #ccc;
    color: #333;
}

.sidebar_right {
    width: 77%;
    background-color: var(--bg-colorWhite);
    margin-top: 20px;
    height: 95%;
    overflow-y: auto;
    border-radius: 15px;
    padding: 20px;
}
</style>










































<!-- <template>
    <div class="infor">
        <div class="sidebar_left">

            <div class="name_img">GoWheels Xin chào</div>

            <hr width="100%" style="margin-block: 10px;color: #ccc;">

            <button style="margin-top: 0;" :class="{ click: page == 0 }"
                @click="page = 0; this.$router.push({ name: 'user-profile-inforuser' });">
                Thông tin cá nhân
            </button>
            <button :class="{ click: page == 1 }" 
                @click="page = 1; this.$router.push({ name: 'user-profile-mycar' });">
                Xe cho thuê của bạn
            </button>
            <button :class="{ click: page == 2 }"
                @click="page = 2; this.$router.push({ name: 'user-profile-historyInvoice' });">
                Lịch sử giao dịch
            </button>
            <button :class="{ click: page == 3 }"
                @click="page = 3; this.$router.push({ name: 'user-profile-historyBooking' });">
                Lịch sử đặt xe
            </button>
            <button :class="{ click: page == 4 }"
                @click="page = 4; this.$router.push({ name: 'user-profile-favoriteList' });">
                Danh sách yêu thích
            </button>
            <button :class="{ click: page == 5 }"
                @click="page = 5; this.$router.push({ name: 'user-profile-voucherList' });">
                Danh sách khuyến mãi
            </button>
            <button :class="{ click: page == 6 }"
                @click="page = 6; this.$router.push({ name: 'user-profile-changEmail' });">
                Thay đổi Email
            </button>
            <button :class="{ click: page == 7 }"
                @click="page = 7; this.$router.push({ name: 'user-profile-changPassword' });">
                Thay đổi password
            </button>
            <button :class="{ click: page == 8 }" @click="page = 8; this.$router.push({ name: 'user-profile-mycar' });">
                Yêu cầu xóa tài khoản
            </button>
            <button :class="{ click: page == 9 }">Đăng xuất</button>
        </div>
        <div class="sidebar_right">
            <router-view></router-view>
        </div>
    </div>
</template>

<script>


export default {
    data() {
        return {
            page: 0,
            user: []
        }
    },
    methods: {
    },
    created() {
        const userString = sessionStorage.getItem('User');
        this.user = JSON.parse(userString);
    },

}
</script>

<style>
.name_img {
    padding: 10px 20px;
    margin-top: 5px;
    font-size: 18px;
    color: #ccc;
}

.infor {
    margin-top: 1px;
    width: 100%;
    display: flex;
    height: 643px;
}

.sidebar_left {
    width: 25%;
    /* background-color: #333; */
}

.sidebar_left button {
    width: 100%;
    text-align: left;
    padding: 15px 10px;
    background-color: #f6f6f6;
    color: #ccc;
    /* border: 0.2px solid #ccc; */
    border: none;
    float: left;
}

.sidebar_left router-link {
    width: 100%;
    text-align: left;
    padding: 15px 10px;
    background-color: #f6f6f6;
    color: #ccc;
    /* border: 0.2px solid #ccc; */
    border: none;
    float: left;
}

.sidebar_left button.click {
    background-color: #ccc;
    color: #333;
}

.sidebar_right {
    width: 75%;
    background-color: blue;
    height: 100%;
    overflow-y: auto;
}
</style> -->