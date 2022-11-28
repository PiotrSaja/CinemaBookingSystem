<template>
  <section class="layout" v-if="isUserLoggedIn">
    <div class="sidebar"><navbar /></div>
    <div class="body">
      <div class="row">
        <menubar1 />
      </div>
      <div class="row p-2">
        <div class="surface-ground px-4 py-5 md:px-6 lg:px-8">
          <div class="grid">
            <div class="col-12 md:col-6 lg:col-6">
              <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                  <div>
                    <span class="block text-500 font-medium mb-3" v-bind:href="'/bookings'"
                      >Bookings</span
                    >
                    <div class="text-900 font-medium text-xl">
                      {{ statistics.numberOfBookings }}
                    </div>
                  </div>
                  <div
                    class="
                      flex
                      align-items-center
                      justify-content-center
                      bg-blue-100
                      border-round
                    "
                    style="width: 2.5rem; height: 2.5rem"
                  >
                    <i class="pi pi-shopping-cart text-blue-500 text-xl"></i>
                  </div>
                </div>
                <span class="text-green-500 font-medium"
                  >{{ statistics.numberOfTodayBookings }} new
                </span>
                <span class="text-500">since yesterday</span>
              </div>
            </div>
            <div class="col-12 md:col-6 lg:col-6">
              <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                  <div>
                    <span class="block text-500 font-medium mb-3" v-bind:href="'/bookings'">Revenue</span>
                    <div class="text-900 font-medium text-xl">
                      ${{ statistics.revenueAll.toFixed(2) }}
                    </div>
                  </div>
                  <div
                    class="
                      flex
                      align-items-center
                      justify-content-center
                      bg-orange-100
                      border-round
                    "
                    style="width: 2.5rem; height: 2.5rem"
                  >
                    <i class="pi pi-dollar text-orange-500 text-xl"></i>
                  </div>
                </div>
                <span class="text-green-500 font-medium"
                  >+{{ (statistics.revenueAll - statistics.revenueMinusOneWeek).toFixed(2) }}
                </span>
                <span class="text-500"> since last week</span>
              </div>
            </div>
            <div class="col-12 md:col-6 lg:col-6">
              <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                  <div>
                    <span class="block text-500 font-medium mb-3" v-bind:href="'/bookings'"
                      >Customers</span
                    >
                    <div class="text-900 font-medium text-xl">
                      {{ statistics.numberOfCustomers }}
                    </div>
                  </div>
                  <div
                    class="
                      flex
                      align-items-center
                      justify-content-center
                      bg-cyan-100
                      border-round
                    "
                    style="width: 2.5rem; height: 2.5rem"
                  >
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
                    <span class="block text-500 font-medium mb-3" v-bind:href="'/seances'">Seances</span>
                    <div class="text-900 font-medium text-xl">
                      {{ statistics.numberOfSeances }} All
                    </div>
                  </div>
                  <div
                    class="
                      flex
                      align-items-center
                      justify-content-center
                      bg-purple-100
                      border-round
                    "
                    style="width: 2.5rem; height: 2.5rem"
                  >
                    <i class="pi pi-video text-purple-500 text-xl"></i>
                  </div>
                </div>
                <span class="text-green-500 font-medium"
                  >{{ statistics.numberOfTodaySeances }}
                </span>
                <span class="text-500"> seances today</span>
              </div>
            </div>
            <div class="col-12 md:col-12 lg:col-12">
              <div class="surface-card shadow-2 p-3 border-round">
                <div class="flex justify-content-between mb-3">
                  <div>
                    <span class="block text-500 font-medium mb-3"
                      >Line Chart</span
                    >
                  </div>
                </div>
                <Chart type="line" :data="basicData" :options="basicOptions" />
                <div class="mt-4">
                  <Dropdown v-model="selectedChart" :options="chartsOptions" optionLabel="name" optionValue="value" placeholder="Select type of chart" />&nbsp;
                  <Dropdown v-if="selectedChart==='BookingsInMonth' || selectedChart==='RevenueInMonth'" v-model="selectedMonth" :options="monthOptions" optionLabel="name" optionValue="value" placeholder="Select a month" />&nbsp;
                  <Button label="Show" @click="getChartData()"/><br><br>
                </div>
                <div v-if="selectedChart==='Bookings' || selectedChart==='Revenue'">
                  Optional filters <Calendar v-model="from" view="month" dateFormat="mm/yy" placeholder="Date from"/>&nbsp;-
                  <Calendar v-model="to" view="month" dateFormat="mm/yy" placeholder="Date to"/>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import moment from 'moment'
import Navbar from "@/components/Navbar.vue";
import Menubar1 from "@/components/Menubar.vue";
import Calendar from "primevue/calendar";
import Dropdown from "primevue/dropdown";
import Chart from "primevue/chart";
import StatisticsService from "@/services/statistics-service";
import Button from "primevue/button";
export default {
  name: "HomeView",
  data() {
    return {
      isUserLoggedIn: false,
      selectedChart: null,
      selectedMonth: null,
      chartsOptions: [
        {
          name: "Bookings in year",
          value: 'Bookings'
        },
        {
          name: "Revenue in year",
          value: 'Revenue'
        },
        {
          name: "Bookings in month",
          value: 'BookingsInMonth'
        },
        {
          name: "Revenue in month",
          value: 'RevenueInMonth'
        }
      ],
      monthOptions: [
        {
          name: "January",
          value: '1'
        },
        {
          name: "February",
          value: '2'
        },
        {
          name: "March",
          value: '3'
        },
        {
          name: "April",
          value: '4'
        },
        {
          name: "May",
          value: '5'
        },
        {
          name: "June",
          value: '6'
        },
        {
          name: "July",
          value: '7'
        },
        {
          name: "August",
          value: '8'
        },
        {
          name: "September",
          value: '9'
        },
        {
          name: "October",
          value: '10'
        },
        {
          name: "November",
          value: '11'
        },
        {
          name: "December",
          value: '12'
        }
      ],
      from: null,
      to: null,
      profile: null,
      statistics: null,
      basicData: {
        labels: [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December",
        ],
        datasets: [
          {
            label: "Bookings",
            data: null,
            fill: false,
            borderColor: "#42A5F5",
            tension: 0.4,
          },
          {
            label: "Revenue $",
            data: null,
            fill: false,
            borderColor: "#FFA726",
            tension: 0.4,
          },
        ],
      },
    };
  },
  components: {
    Chart,
    Navbar,
    Menubar1,
    Calendar,
    Dropdown,
    Button
  },
  created() {
    StatisticsService.getAll()
      .then((response) => {
        this.statistics = response.data;
      })
      .catch((error) => {
        console.log(error.response.data);
      });
    this.$auth
      .isUserLoggedIn()
      .then((isLoggedIn) => {
        this.isUserLoggedIn = isLoggedIn;
      })
      .catch((error) => {
        console.log(error);
        this.isUserLoggedIn = false;
      });
    this.$auth
      .getProfile()
      .then((profile) => {
        this.profile = profile;
      })
      .catch((error) => {
        console.log(error);
        this.profile = {};
      });
  },
  methods: {
    onLogin() {
      this.$auth.login();
    },
    getChartData(){
      this.basicData.datasets[0].data = null
      this.basicData.datasets[1].data = null

      StatisticsService.get(this.from !== null ? moment(String(this.from)).format('YYYY-MM-DD') : "", this.to !== null ? moment(String(this.to)).format('YYYY-MM-DD') : "", this.selectedMonth != null ? this.selectedMonth : '')
      .then((response) => {
        this.statistics = response.data;
      })
      .catch((error) => {
        console.log(error.response.data);
      });
      
      StatisticsService.getChartData(this.selectedChart, this.from !== null ? moment(String(this.from)).format('YYYY-MM-DD') : "", this.to !== null ? moment(String(this.to)).format('YYYY-MM-DD') : "", this.selectedMonth != null ? this.selectedMonth : '')
      .then((response) => {
        if(this.selectedChart  === "Bookings"){
          this.basicData.datasets[0].data = response.data.dataToChart;
          let labelsTemp = [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December",
        ]
          this.basicData.labels = labelsTemp;
        }
        if(this.selectedChart  === "Revenue"){
          this.basicData.datasets[1].data = response.data.dataToChart;
          let labelsTemp = [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June",
          "July",
          "August",
          "September",
          "October",
          "November",
          "December",
        ]
          this.basicData.labels = labelsTemp;
        }
        if(this.selectedChart  === "BookingsInMonth"){
          this.basicData.datasets[0].data = response.data.dataToChart;
          let labelsTemp = [
          "1",
          "2",
          "3",
          "4",
          "5",
          "6",
          "7",
          "8",
          "9",
          "10",
          "11",
          "12",
          "13",
          "14",
          "15",
          "16",
          "17",
          "18",
          "19",
          "20",
          "21",
          "22",
          "23",
          "24",
          "25",
          "26",
          "27",
          "28",
          "29",
          "30",
          "31",
        ]
          this.basicData.labels = labelsTemp;
        }
        if(this.selectedChart  === "RevenueInMonth"){
          this.basicData.datasets[1].data = response.data.dataToChart;
          let labelsTemp = [
          "1",
          "2",
          "3",
          "4",
          "5",
          "6",
          "7",
          "8",
          "9",
          "10",
          "11",
          "12",
          "13",
          "14",
          "15",
          "16",
          "17",
          "18",
          "19",
          "20",
          "21",
          "22",
          "23",
          "24",
          "25",
          "26",
          "27",
          "28",
          "29",
          "30",
          "31",
        ]
          this.basicData.labels = labelsTemp;
        }
      })
      .catch((error) => {
        console.log(error.response.data);
      });
    }
  },
};
</script>

<style scoped>
[v-cloak] {
  display: none;
}
.layout {
  width: 100%;
  display: grid;
  grid:
    "sidebar body" 1fr
    / 15% 85%;
  gap: 8px;
}
.sidebar {
  grid-area: sidebar;
  min-height: 100vh;
  border-right: 1px solid lightgray;
}
.body {
  grid-area: body;
}
</style>
