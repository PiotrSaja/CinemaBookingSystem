<template>
    <div class="page text-white">
      <div class="background-image" style="background-image: url('https://wallpaperaccess.com/full/1773889.jpg')">
      </div>
      <h5 class="underline font-weight-bold mt-4">Cinema detail</h5>
      <div class="row mt-4">
        <div class="col-6">
          <h3>{{cinema.name}}</h3>
          <h5>Total cinema halls: {{cinema.totalCinemaHalls}}</h5><br>
          <h5>Address</h5>
          <h6>st. {{cinema.street}}</h6>
          <h6>{{cinema.city}}, {{cinema.zipCode}}</h6>
          <h6>{{cinema.country}}</h6>
        </div>
        <div class="col-6 text-right">
          <h5>Contact</h5>
          <h6>Email: contact@email.com</h6>
          <h6>Phone: +48 123456789</h6>
        </div>
      </div>
      <div class="row mt-5">
        <subsequent-seances-in-cinema/>
      </div>
    </div>
</template>

<script>
import CinemaService from '@/api-services/cinema-service'
import SubsequentSeancesInCinema from '@/components/SubsequentSeancesInCinema.vue'

export default {
  components: { SubsequentSeancesInCinema },
  name: 'CinemaDetail',
  data () {
    return {
      cinema: {},
      errorMessage: ''
    }
  },
  created () {
    CinemaService.get(this.$router.currentRoute.params.id).then((response) => {
      this.cinema = response.data
    }).catch(error => {
      if (error.response.status === 404) {
      this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
      }
    })
  }
}
</script>

<style scoped>
.page {
min-height: 100vh;
}
.background-image{
  height: 350px;
  min-height: 350px;
  width: 100%;
  background-repeat: no-repeat;
  background-position: bottom center;
  background-size: cover;
}
.underline {
  margin-top: 15px;
  margin-bottom: 30px;
  text-align: center;
  width:150px;
  border-bottom: 2px solid #FF9100;
  position: relative;
  left: 50%;
  transform: translateX(-50%);
}
</style>
