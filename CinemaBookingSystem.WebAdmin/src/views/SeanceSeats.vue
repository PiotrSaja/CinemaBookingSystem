<template>
    <card class="mt-3">
        <template #title>
            Seats configuration
        </template>
        <template #content>
            <div class="ml-2">
                <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
                    <div class="card">
                        <h4>Seance Information:</h4>
                        <span>Date: {{seance.date}}</span><br>
                        <span>Movie: {{seance.movie.title}}</span><br>
                        <span>Cinema Hall: {{seance.cinemaHall.name}}</span>
                        <table class="mt-3">
                        <tr>
                            <th>Row</th>
                            <th>Seat number</th>
                            <th>Seat Type</th>
                            <th>Price</th>
                        </tr>
                        <tr v-for="(seat, index) in cinemaSeats.items" :key="seat.id">
                            <td>{{seat.row}}</td>
                            <td>{{seat.seatNumber}}</td>
                            <td>
                                <span v-if="seat.seatType === 1">Normal</span>
                                <span v-else>Vip</span>
                            </td>
                            <td><InputNumber id="price" v-model="seanceSeatsPrice[index]" mode="decimal" :minFractionDigits="2" :allowEmpty="false"/></td>
                        </tr>
                        </table>
                    </div>
                    <div class="text-right row mt-5">
                        <Button label="Submit" icon="pi pi-check" iconPos="left" class="p-button-success mr-2"  @click="submit()"/>
                        <Button label="Cancel" icon="pi pi-times" iconPos="left" class="p-button-info mr-2" @click="goBack()" />
                    </div>
                </div>
        </template>
    </card>
</template>

<script>
import InputNumber from 'primevue/inputnumber';

import Message from 'primevue/message';
import Button from 'primevue/button';
import SeanceService from '@/services/seance-service';
import CinemaSeatsService from '@/services/cinema-seats-service';
import SeanceSeatsService from '@/services/seance-seats-service';
import { useRoute } from 'vue-router';
import Card from 'primevue/card';
export default {
    name: 'SeanceSeatsView',
    components: {
        Button,
        Message,
        Card,
        InputNumber
    },
    data() {
        return {
            displayConfirmation: false,
            showError: false,
            errorMessage: '',
            seance: {},
            cinemaSeats: {},
            seanceSeatsPrice: []
        }
    },
    created () {
        const route = useRoute(); 
        const id = route.params.id;
        SeanceService.get(id).then((response) => {
            this.seance = response.data

            CinemaSeatsService.getAll(response.data.cinemaHall.id).then((response) => {
            this.cinemaSeats = response.data
            }).catch(error => {
            if (error.response.status === 404) {
                this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
            }
        })
            }).catch(error => {
            if (error.response.status === 404) {
                this.$router.replace({name: 'NotFound', params: {err: error.response.data.Message}})
            }
        })
        
    },
    methods: {
        openConfirmation() {
            this.displayConfirmation = true;
        },
        closeConfirmation() {
            this.displayConfirmation = false;
        },
        goBack() {
            this.$router.push({name: 'Seances'})
        },
        createSeanceSeats () {
            const seanceSeats = [];

            for(var i=0; i < this.cinemaSeats.items.length; i++){
                var seanceSeat = {
                    cinemaSeatId: this.cinemaSeats.items[i].id,
                    price: this.seanceSeatsPrice[i]
                }
                seanceSeats.push(seanceSeat)
            }
            const request = {
                seanceId: this.seance.id,
                seanceSeats: seanceSeats
            }
            SeanceSeatsService.createSeats(request).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'Seances'})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        submit () {
            this.createSeanceSeats();
        }
    }
}
</script>

<style scoped lang="scss">
a {
    text-decoration: none;
    color: black;
}
.field * {
    display: block;
}
table {
  border-collapse: collapse;
  width: 100%;
}

td, th {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

tr:nth-child(even) {
  background-color: #dddddd;
}
</style>