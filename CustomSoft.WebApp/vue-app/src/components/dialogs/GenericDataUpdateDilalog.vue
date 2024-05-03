<template>
    <div class="text-center pa-4">
        <v-dialog v-model="dialog"
                  max-width="400"
                  persistent>
            <v-card prepend-icon="mdi-map-marker"
                    text="{{ bodyText }}"
                    title="{{ title }}">
                <template v-slot:actions>
                    <v-spacer></v-spacer>

                    <v-btn @click="$emit('cancel')">
                        Cancelar
                    </v-btn>

                    <v-btn @click="$emit('accept')">
                        Aceptar
                    </v-btn>
                </template>
            </v-card>
        </v-dialog>
    </div>
</template>
<script>
    export default {
        emits: ['accept', 'cancel'],
        data() {
            return {
                dialog: false,
            }
        },
        props: {
            bodyText: String,
            title: String
        },
        methods: {
            showDialog() {
                this.dialog = true;
            },
            hiddeDialog() {
                this.dialog = false;
            },
        }
    }
</script>