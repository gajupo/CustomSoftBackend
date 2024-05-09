<template>
    <v-dialog v-model="dialog"
              max-width="800">
        <v-card prepend-icon="mdi-file"
                title="Exportar Proveedores">
            <v-card-text>
                <v-form ref="form">
                    <v-row dense class="mb-5">
                        <v-col cols="6">
                            <ExportDataPicker id=""startDate label="Fecha Inicial"
                                              v-model="startDate"
                                              :rules="[rules.required]"
                                              color="primary"></ExportDataPicker>
                        </v-col>
                        <v-col cols="6">
                            <ExportDataPicker id="endDate" label="Fecha Final"
                                              v-model="endDate"
                                              :rules="[rules.required]"
                                              color="primary"></ExportDataPicker>
                        </v-col>
                    </v-row>
                    <v-alert v-if="errorMessage" type="error" closable>{{ errorMessage }}</v-alert>
                </v-form>
            </v-card-text>

            <v-divider></v-divider>

            <v-card-actions>
                <v-spacer></v-spacer>
                <v-progress-circular v-show="exportingFile" color="primary" class="mt-2"
                        indeterminate>
                    </v-progress-circular>
                <v-btn text="Cerrar"
                       variant="plain"
                       @click="close"></v-btn>
                <v-btn color="primary"
                       text="Generar"
                       variant="tonal"
                       @click="validate"></v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    import ExportDataPicker from './ExportDatePicker.vue'
    import { parse } from 'date-fns'
    import { toZonedTime } from 'date-fns-tz'

    export default {
        components: {
            ExportDataPicker
        },
        props: ['modelValue'],
        emits: ['update:modelValue'],
        data: () => ({
           startDate: null,
           endDate: null,
           rules: {
                required: value => !!value || 'Required.',
            },
            errorMessage: '',
            exportingFile: false,
        }),
        computed: {
            dialog: {
                get() {
                    return this.modelValue;
                },
                set(val) {
                    this.$emit('update:modelValue', val);
                }
            }
        },
        methods: {
            close() {
                this.$emit('update:modelValue', false);
            },
            generateReport() {
                console.log("Generating report from", this.startDate);
            },
            async validate() {
                const { valid } = await this.$refs.form.validate()
                this.errorMessage = ''
                if(this.startDate && this.endDate){
                    const sDate = this.convertToDate(this.startDate)
                    const eDate = this.convertToDate(this.endDate)
                    if(sDate > eDate){
                        this.errorMessage = 'La fecha inicial no puede ser mayor que la final';
                    } else {
                        await this.generateReport(toZonedTime(sDate,'America/Mexico_City').toISOString(), toZonedTime(eDate,'America/Mexico_City').toISOString())
                    }
                }
            },
            convertToDate(dateString) {
                let date = parse(dateString, 'dd/MM/yyyy', new Date());
                
                if (isNaN(date)) {
                    date = parse(dateString, 'dd/M/yyyy', new Date());
                }

                return date;
            },
            async generateReport(startDate, endDate) {
                this.exportingFile = true
                const result = await this.$api.getAsBlob(`/proveedores/export-xlsx?startDate=${startDate}&endDate=${endDate}`)

                const url = URL.createObjectURL(result)
                const link = document.createElement('a')
                link.href = url
                link.download = `Proveedores${this.startDate.replace('\\', '-')} - ${this.endDate.replace('\\', '-')}.xlsx`
                link.click()

                this.exportingFile = false
                this.$refs.form.reset()
            }
        }
    }
</script>
