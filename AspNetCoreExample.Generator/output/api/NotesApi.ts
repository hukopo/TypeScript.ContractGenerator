// eslint-disable
// TypeScriptContractGenerator's generated content
import { BlogEntry } from './../dto/BlogEntry';
import { UserApiBase } from './../apiBase/UserApiBase';

export class NotesApi extends UserApiBase implements INotesApi {
    async addEntry(entry: BlogEntry): Promise<void> {
        return this.post(`blog/`, {
            
        }, {
            ...entry,
        });
    }

    async addEntries(entries: BlogEntry[]): Promise<void> {
        return this.post(`blog/batch`, {
            
        }, entries);
    }

};
export interface INotesApi {
    addEntry(entry: BlogEntry): Promise<void>;
    addEntries(entries: BlogEntry[]): Promise<void>;
}
