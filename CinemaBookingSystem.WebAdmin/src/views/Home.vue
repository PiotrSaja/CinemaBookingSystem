<template>
<div class="surface-ground px-4 py-5 md:px-6 lg:px-8">
    <div class="grid">
        <div class="col-12 md:col-6 lg:col-6">
            <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Bookings</span>
                        <div class="text-900 font-medium text-xl">{{statistics.numberOfBookings}}</div>
                    </div>
                    <div class="flex align-items-center justify-content-center bg-blue-100 border-round" style="width:2.5rem;height:2.5rem">
                        <i class="pi pi-shopping-cart text-blue-500 text-xl"></i>
                    </div>
                </div>
                <span class="text-green-500 font-medium">{{statistics.numberOfTodayBookings}} new </span>
                <span class="text-500">since yesterday</span>
            </div>
        </div>
        <div class="col-12 md:col-6 lg:col-6">
            <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Revenue</span>
                        <div class="text-900 font-medium text-xl">${{statistics.revenueAll}}</div>
                    </div>
                    <div class="flex align-items-center justify-content-center bg-orange-100 border-round" style="width:2.5rem;height:2.5rem">
                        <i class="pi pi-dollar text-orange-500 text-xl"></i>
                    </div>
                </div>
                <span class="text-green-500 font-medium">+{{statistics.revenueAll - statistics.revenueMinusOneWeek}} </span>
                <span class="text-500"> since last week</span>
            </div>
        </div>
        <div class="col-12 md:col-6 lg:col-6">
            <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Customers</span>
                        <div class="text-900 font-medium text-xl">{{statistics.numberOfCustomers}}</div>
                    </div>
                    <div class="flex align-items-center justify-content-center bg-cyan-100 border-round" style="width:2.5rem;height:2.5rem">
                        <i class="pi pi-users text-cyan-500 text-xl"></i>
                    </div>
                </div>
                <span class="text-500">In system</span>
            </div>
        </div>
        <div class="col-12 md:col-6 lg:col-6">
            <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Seances</span>
                        <div class="text-900 font-medium text-xl">{{statistics.numberOfSeances}} All</div>
                    </div>
                    <div class="flex align-items-center justify-content-center bg-purple-100 border-round" style="width:2.5rem;height:2.5rem">
                        <i class="pi pi-video text-purple-500 text-xl"></i>
                    </div>
                </div>
                <span class="text-green-500 font-medium">{{statistics.numberOfTodaySeances}} </span>
                <span class="text-500"> seances today</span>
            </div>
        </div>
        <div class="col-12 md:col-12 lg:col-12">
            <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Line Chart</span>
                    </div>
                </div>
                <Chart type="line" :data="basicData" :options="basicOptions" />
            </div>
        </div>
    </div>
</div>
</template>

<script>
import Chart from 'primevue/chart';
import StatisticsService from '@/services/statistics-service';
export default {
  name: 'HomeView',
  data () {
    return {
      isUserLoggedIn: false,
      profile: null,
      statistics: null,
      basicData: {
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                datasets: [
                    {
                        label: 'Bookings',
                        data: [65, 59, 80, 81, 56, 55, 40, 100, 200, 50, 20, 47],
                        fill: false,
                        borderColor: '#42A5F5',
                        tension: .4
                    },
                    {
                        label: 'Revenue $',
                        data: [28, 48, 40, 19, 86, 27, 90, 127, 341, 89, 45, 60],
                        fill: false,
                        borderColor: '#FFA726',
                        tension: .4
                    }
                ]
            }
    }
  },
  components: {
    Chart
  },
  created () {
    StatisticsService.get().then((response) => {
        this.statistics = response.data

        this.basicData.datasets[0].data = this.statistics.bookingsInMonths
        this.basicData.datasets[1].data = this.statistics.revenueInMonths
        }).catch((error) => {
        console.log(error.response.data)
    })
    this.$auth.isUserLoggedIn()
      .then(isLoggedIn => {
        this.isUserLoggedIn = isLoggedIn
      })
      .catch(error => {
        console.log(error)
        this.isUserLoggedIn = false
      })
    this.$auth.getProfile()
      .then(profile => {
        this.profile = profile
      })
      .catch(error => {
        console.log(error)
        this.profile = {}
      })
  },
  methods: {
    onLogin () {
      this.$auth.login()
    },
  }
}
</script>

<style>
</style>
