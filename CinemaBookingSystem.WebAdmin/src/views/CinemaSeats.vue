<template>
    <card class="mt-3">
        <template #title>
            Cinema seats configuration
        </template>
        <template #content>
            <div class="ml-2">
                <Message severity="error" v-if="showError" :closable="false">{{errorMessage}}</Message>
                    <div class="card">
                        <h4>Cinema Hall ID: {{cinemaHallId}}</h4>
                        <div v-for="i in this.cinemaHall.numberOfRows" :key="i">
                            <h5>Row number: {{i}}: Quantity of seats in row: {{this.cinemaHall.numberOfColumns}}</h5>
                                Seat type: <Dropdown v-model="selectedSeatType[i]" :options="seatTypes" optionLabel="name" placeholder="Select a seat type" class="ml-3" required/>
                        </div>
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
import Dropdown from 'primevue/dropdown';
import Message from 'primevue/message';
import Button from 'primevue/button';
import CinemaHallService from '@/services/cinema-hall-service';
import CinemaSeatsService from '@/services/cinema-seats-service';
import { useRoute } from 'vue-router';
import Card from 'primevue/card';
export default {
    name: 'CinemaSeatsView',
    components: {
        Button,
        Message,
        Card,
        Dropdown
    },
    data() {
        return {
            rows: 5,
            columns: 5,
            displayConfirmation: false,
            cinemaHallId: 0,
            selectedSeatType: [],
            seatTypes: [
                {name: 'Normal', code: 0},
                {name: 'Vip', code: 1}
            ],
            showError: false,
            errorMessage: '',
            cinemaHall: {}
        }
    },
    created () {
        const route = useRoute(); 
        const id = route.params.cinemaHallId;
        this.cinemaHallId = route.params.cinemaHallId;
        CinemaHallService.get(id).then((response) => {
            this.cinemaHall = response.data

            for (var i = 1; i <= this.cinemaHall.numberOfColumns; i++) {
                this.selectedSeatType[i] = {name: 'Normal', code: 0};
            }
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
            this.$router.push({name: 'Cinemas'})
        },
        createCinemaSeats () {
            const cinemaSeats = [];

            for (var i = 1; i <= this.cinemaHall.numberOfRows; i++) {
                for (var y = 1; y <= this.cinemaHall.numberOfColumns; y++) {
                    var cinemaSeat = {
                        cinemaHallId: this.cinemaHallId,
                        seatType: this.selectedSeatType[i].code,
                        row: i,
                        seatNumber: y
                    }
                    cinemaSeats.push(cinemaSeat)
                }
            }
            const request = {
                cinemaHallId: this.cinemaHallId,
                cinemaSeats: cinemaSeats
            }

            CinemaSeatsService.createSeats(request).then((response) => {
                console.log(response.data)
                this.$router.replace({name: 'CinemaHalls', params: {id: this.cinemaHallId}})
            }).catch(error => {
                console.log(error)
                this.errorMessage = error.response.data.Message;
                this.showError = true;
            })
        },
        submit () {
            this.createCinemaSeats();
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
</style>