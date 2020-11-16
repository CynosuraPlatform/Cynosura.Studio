﻿import { SolutionFilter } from './solution-filter.model';

export class Solution {
    id: number;
    creationDate: Date;
    modificationDate: Date;
    creationUserId: number;
    modificationUserId: number;
    name: string;
    path: string;
    templateName: string;
    templateVersion: string;
}

export class SolutionListState {
    pageSize = 10;
    pageIndex = 0;
    filter = new SolutionFilter();
}
