import { Customer } from "./customer";

export interface Trip {
    id: number;
    destination: string;
    participants: Customer[];
    isCancelled: boolean;
    owner: string;
}